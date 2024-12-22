--Admin related queries and stored procedures
use railway_db;



create or alter procedure sp_checkIfTrainExists
	@TrainNum int,
	@Exists bit output
as
begin
	if exists (select 1 from train_table where Train_Number = @TrainNum and Status = 'Active')
	begin
		set @Exists = 1;
	end
	else
	begin
		set @Exists = 0;
	end
end

----------------------------------------------------------------------------------------------------------------------------------
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'train_table';
update train_table set Train_Number = 54321 WHERE Train_Number = 23341;
----------------------------------------------------------------------------------------------------------------------------------

create procedure sp_checkTrainStatus
	@TrainNum int,
	@StatusNum int output
as
begin
	 
	DECLARE @stat VARCHAR(10);

    -- Get the train's status
    SELECT @stat = Status 
    FROM train_table 
    WHERE Train_Number = @TrainNum;

    -- Check if the train's status is Active
    IF (@stat = 'Active')
    BEGIN
        -- Check if the train is in an active schedule
        IF EXISTS (SELECT 1 
                   FROM train_Schedule_Details 
                   WHERE TrainNo = @TrainNum AND Status = 'Active')
        BEGIN
            SET @StatusNum = 1; -- Train is in schedule and active
        END
        ELSE IF EXISTS (SELECT 1 
                        FROM train_Schedule_Details 
                        WHERE TrainNo = @TrainNum AND Status = 'Cancelled')
        BEGIN
            SET @StatusNum = 2; -- Train is in schedule but cancelled
        END
        ELSE
        BEGIN
            SET @StatusNum = 3; -- Train is not found in any schedule
        END
    END
    ELSE
    BEGIN
        -- Train is not active or not scheduled
        SET @StatusNum = 4; 
    END
end
----------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_UpdateTrainNumber
    @OldTrainNum INT,
    @NewTrainNum INT,
    @Result BIT OUTPUT
AS
BEGIN
	Declare @TrainName VARCHAR(100);    -- New column for Train Name
    Declare @TrainType VARCHAR(50);    -- New column for Train Type
    Declare @Status VARCHAR(15);        -- New column for Status
    -- Start transaction
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Step 1: Check if the Old Train Number exists collect data into variables
        select @TrainName = Train_Name from train_table where Train_Number = @OldTrainNum; 
		select @TrainType = Train_Type from train_table where Train_Number = @OldTrainNum;
		select @Status = Status from train_table where Train_Number = @OldTrainNum;

        -- Step 2: Check if the New Train Number already exists
        IF EXISTS (SELECT 1 FROM train_table WHERE Train_Number = @NewTrainNum)
        BEGIN
            -- Update the New Train Number's details if it already exists
            UPDATE train_table
            SET Train_Name = @TrainName,
                Train_Type = @TrainType,
                Status = @Status
            WHERE Train_Number = @NewTrainNum;
        END
        ELSE
        BEGIN
            -- Insert the new Train Number into the train_table
            INSERT INTO train_table (Train_Number, Train_Name, Train_Type, Status)
            VALUES (@NewTrainNum, @TrainName, @TrainType, @Status);
        END;

        -- Step 3: Update the child table (train_Schedule_Details) with the new Train Number
        UPDATE train_Schedule_Details
        SET TrainNo = @NewTrainNum
        WHERE TrainNo = @OldTrainNum;

        -- Step 4: Delete the Old Train Number from the parent table
        DELETE FROM train_table
        WHERE Train_Number = @OldTrainNum;

        -- Commit the transaction
        COMMIT TRANSACTION;

        SET @Result = 1; -- Success
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of errors
        ROLLBACK TRANSACTION;

        SET @Result = 0; -- Failure
        THROW;
    END CATCH
END;

----
DECLARE @output BIT;

-- Test the procedure with example data
EXEC sp_UpdateTrainNumber 
    @OldTrainNum = 14331, 
    @NewTrainNum = 12345, 
    @Result = @output OUTPUT;

-- Check the output
SELECT @output AS Result;

----------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_checkIfTrainExistInTrainScheduleAndChangeIt
    @OldTrainNum INT,
    @NewTrainNum INT,
    @Exists BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        -- Check if the old Train_Number exists in train_Schedule_Details
        IF EXISTS (
            SELECT 1
            FROM train_Schedule_Details
            WHERE TrainNo = @OldTrainNum
        )
        BEGIN
            -- Update the Train_Number in the child table
            UPDATE train_Schedule_Details
            SET TrainNo = @NewTrainNum
            WHERE TrainNo = @OldTrainNum;

            SET @Exists = 1; -- Train exists and was updated
        END
        ELSE
        BEGIN
            SET @Exists = 0; -- No Train instance found
        END;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

----------------------------------------------------------------------------------------------------------------------------------
create or alter procedure sp_checkifTrainInstanceExistsBoth --not only active but also cancelled too
	@TrainInstance int,
	@Exists bit output
as
begin
	if exists (select 1 from train_Schedule_Details where TrainInstanceId = @TrainInstance)
	begin
		set @Exists = 1;
	end
	else
	begin
		set @Exists = 0;
	end
end
----------------------------------------------------------------------------------------------------------------------------------
create or alter procedure sp_checkifTrainInstanceExistsOnlyActive 
	@TrainInstance int,
	@Exists bit output
as
begin
	if exists (select 1 from train_Schedule_Details where TrainInstanceId = @TrainInstance and Status = 'Active')
	begin
		set @Exists = 1;
	end
	else
	begin
		set @Exists = 0;
	end
end
----------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_checkTrainScheduleCollision
    @TrainNumber INT,
    @DateOfDeparture DATE,
    @Exists BIT OUTPUT
AS
BEGIN
    SET @Exists = 0; --no collision

    -- Check for collision
    IF EXISTS (
        SELECT 1 
        FROM train_Schedule_Details 
        WHERE TrainNo = @TrainNumber 
          AND DateOfDeparture = @DateOfDeparture
          AND Status = 'Active'
    )
    BEGIN
        SET @Exists = 1; -- Collision exists
    END
END;
----------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_addTrainSchedule
    @TrainInstanceId INT,
    @TrainNumber INT,
    @from VARCHAR(30),
    @to VARCHAR(30),
    @timings TIME,
    @HOJ INT,
    @DOD DATE
AS
BEGIN
    -- Insert into train_Schedule_Details
    INSERT INTO train_Schedule_Details
        (TrainInstanceId, TrainNo, _From, _To, Timings, HoursOfJourney, DateOfDeparture, Status)
    VALUES
        (@TrainInstanceId, @TrainNumber, @from, @to, @timings, @HOJ, @DOD, 'Active');

    PRINT 'Train schedule added successfully.';
END;
----------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_CancelTrainScheduleAndRelevantTickets
    @TrainInstanceId INT,
	@Result bit output
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Soft delete the train schedule (mark it as cancelled)
        UPDATE train_Schedule_Details
        SET Status = 'Cancelled'
        WHERE TrainInstanceId = @TrainInstanceId;

        -- Update all tickets linked to the cancelled train schedule
        UPDATE ticket_table
        SET Status = 'Cancelled',
            Remarks = 'Train Schedule is cancelled'
        WHERE TrainInstanceId = @TrainInstanceId;

        COMMIT TRANSACTION;
		SET @Result = 1;
        PRINT 'Train schedule and tickets updated successfully.';
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        ROLLBACK TRANSACTION;
		SET @Result = 0;
        PRINT 'An error occurred. Transaction rolled back.';
        THROW;
    END CATCH
END;

----------------------------------------------------------------------------------------------------------------------------------
