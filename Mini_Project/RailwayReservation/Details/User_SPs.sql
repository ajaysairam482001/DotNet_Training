--all user sp written here
use railway_db;

create or alter procedure sp_checkifUserExists
	@userId int,
	@Exists bit output
as
begin
	if exists (select 1 from user_table where id = @userId)
	begin
		set @Exists = 1
	end
	else
	begin
		set @Exists = 0;
	end
end
----------------------------------------------------------------------------------------------------------------------------------
CREATE or alter PROCEDURE sp_BookTicketsWithCancelledCheck
    @TrainInstanceId INT,
    @UserId INT,
    @Class VARCHAR(20),
    @RequestedTickets INT,
    @BookingDate DATE,
    @Price DECIMAL(10, 2),
    @Remarks VARCHAR(255) OUTPUT
AS
BEGIN
    DECLARE @AvailableTickets INT;
    DECLARE @CancelledTicketsCount INT;
    DECLARE @SeatMaxLimit INT;
    DECLARE @NumberOfBerthAvailable INT;
    DECLARE @SeatNumber INT;

    -- Start a transaction
    BEGIN TRANSACTION;

    -- Check for canceled tickets
    SELECT TOP (@RequestedTickets) TicketNumber
    INTO #CancelledTickets
    FROM ticket_table
    WHERE TrainInstanceId = @TrainInstanceId AND Class = @Class AND Status = 'Cancelled';

    SET @CancelledTicketsCount = @@ROWCOUNT;

    -- If there are canceled tickets, reassign them to the user
    IF @CancelledTicketsCount > 0
    BEGIN
        UPDATE ticket_table
        SET User_ID = @UserId,
            Status = 'Active',
            Remarks = 'Reassigned to User due to cancelled ticket'
        WHERE TicketNumber IN (SELECT TicketNumber FROM #CancelledTickets);

        SET @RequestedTickets = @RequestedTickets - @CancelledTicketsCount;
    END

    -- If there are still tickets to book, check availability
    IF @RequestedTickets > 0
    BEGIN
        -- Retrieve seat max limit, number of available berths, and current available seats
        SELECT @SeatMaxLimit = SeatLimit,
               @NumberOfBerthAvailable = NumberOfAvailableBerth,
               @AvailableTickets = AvailableSeats
        FROM BerthDetails_table
        WHERE TrainInstanceNo = @TrainInstanceId AND Class = @Class;

        -- Check if sufficient tickets are available
        IF @AvailableTickets < @RequestedTickets
        BEGIN
            ROLLBACK TRANSACTION;
            SET @Remarks = 'Insufficient tickets available';
            RETURN;
        END

        -- Update the available tickets
        UPDATE BerthDetails_table
        SET AvailableSeats = AvailableSeats - @RequestedTickets
        WHERE TrainInstanceNo = @TrainInstanceId AND Class = @Class;

        -- Calculate starting seat number
        SET @SeatNumber = @SeatMaxLimit - @AvailableTickets + 1;

        -- Insert new tickets into the ticket table
        WHILE @RequestedTickets > 0
        BEGIN
            -- Calculate berth type based on the seat number
            DECLARE @Berth VARCHAR(1);
            SET @Berth = CASE (@SeatNumber % @NumberOfBerthAvailable)
                            WHEN 1 THEN 'M' -- Lower
                            WHEN 2 THEN 'U' -- Middle
                            WHEN 0 THEN 'L' -- Upper (Modulo 0 means upper)
                         END;

            -- Insert the ticket
            INSERT INTO ticket_table (TrainInstanceId, User_ID, Class, SeatNumber, Berth, BookingDate, Price)
            VALUES (@TrainInstanceId, @UserId, @Class, @SeatNumber, @Berth, @BookingDate, @Price);

            -- Increment seat number and decrement the requested tickets
            SET @SeatNumber = @SeatNumber + 1;
            SET @RequestedTickets = @RequestedTickets - 1;
        END
    END

    -- Commit transaction
    COMMIT TRANSACTION;

    SET @Remarks = 'Tickets successfully booked';
END;

------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_GetBookingDetails --get you date and price of tickets
    @TrainInstanceId INT,
    @Class VARCHAR(50),
    @DepartureDate DATE OUTPUT,
    @Price DECIMAL(10, 2) OUTPUT
AS
BEGIN
    -- Retrieve the departure date
    SELECT TOP 1 @DepartureDate = DateOfDeparture
    FROM train_schedule_details
    WHERE TrainInstanceId = @TrainInstanceId;

    -- Retrieve the price for the specified class
    SELECT TOP 1 @Price = Price
    FROM TrainInstancePriceDetails
    WHERE TrainInstanceId = @TrainInstanceId AND Class = @Class;
    
END
--------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_GetTicketsByUserAndInstance
    @UserId INT,
    @TrainInstanceId INT
AS
BEGIN
    SELECT 
        TicketNumber,
        TrainInstanceId,
        User_ID,
        Class,
        Berth,
        SeatNumber,
        BookingDate,
        Price
    FROM ticket_table
    WHERE User_ID = @UserId 
      AND TrainInstanceId = @TrainInstanceId
      AND Status = 'Active'
    ORDER BY SeatNumber; 
END;
--------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_CancelTicketsByUserAndInstance
    @UserId INT,
    @TrainInstanceId INT,
	@TicketNumber INT,
    @Remarks VARCHAR(255) OUTPUT
AS
BEGIN
	DECLARE @Price DECIMAL(10, 2);
	DECLARE @RefundAmount DECIMAL(10, 2);
    -- Check if the ticket exists for the user and train instance
    IF EXISTS (
        SELECT 1 
        FROM ticket_table 
        WHERE User_ID = @UserId AND TrainInstanceId = @TrainInstanceId And TicketNumber = @TicketNumber
    )
    BEGIN
        -- Check if the ticket is active
        IF EXISTS (
            SELECT 1 
            FROM ticket_table 
            WHERE User_ID = @UserId AND TicketNumber = @TicketNumber AND TrainInstanceId = @TrainInstanceId AND Status = 'Active'
        )
        BEGIN
			-- Get the ticket price
            SELECT @Price = Price
            FROM ticket_table
            WHERE User_ID = @UserId AND TicketNumber = @TicketNumber AND TrainInstanceId = @TrainInstanceId;
			SET @RefundAmount = @Price * 0.9; --calculate the refunded amt	
            -- Cancel the active ticket
            UPDATE ticket_table
            SET 
                Status = 'Cancelled',
                Remarks = 'Cancelled by User'
            WHERE 
                User_ID = @UserId 
				AND TicketNumber = @TicketNumber
                AND TrainInstanceId = @TrainInstanceId 
                AND Status = 'Active';
				
            SET @Remarks = CONCAT('Ticket successfully cancelled. Refunded amount: Rs.', CAST(@RefundAmount AS VARCHAR));
        END
        ELSE IF EXISTS (
            SELECT 1 
            FROM ticket_table 
            WHERE User_ID = @UserId AND TicketNumber = @TicketNumber AND TrainInstanceId = @TrainInstanceId AND Status = 'Cancelled'
        )
        BEGIN
            -- Ticket is not active
            SET @Remarks = 'Ticket already cancelled. Please enter an active ticket number.';
        END
		ELSE
		BEGIN
			SET @Remarks = 'Ticket present but neither active nor cancelled Please Check!.';
		END
    END
    ELSE
    BEGIN
        -- Ticket does not exist
        SET @Remarks = 'Invalid ticket number. Please enter a valid ticket number.';
    END
END;
--------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_GetActiveTicketsByUserId
    @UserId INT
AS
BEGIN
    
    SELECT 
        TicketNumber,TrainInstanceId,Class,SeatNumber,Berth,BookingDate,Price
    FROM 
        ticket_table
    WHERE 
        User_ID = @UserId AND Status = 'Active'
    ORDER BY 
        TrainInstanceId, 
        SeatNumber;
END;
--------------------------------------------------------------------------------------------------------------------------------------