use railway_db;
--stored Procedures

CREATE OR ALTER PROCEDURE sp_checkLogin
    @UserName VARCHAR(30),
    @Password VARCHAR(100),
    @Role VARCHAR(5) OUTPUT,
    @isSuccess BIT OUTPUT,
    @UserId INT OUTPUT -- New output parameter
AS
BEGIN
    -- Initialize @UserId to 0
    SET @UserId = 0;

    IF EXISTS (SELECT 1 FROM login_table WHERE UserName = @UserName AND Password = @Password)
    BEGIN
        -- Retrieve the role
        SELECT @Role = Category 
        FROM login_table 
        WHERE UserName = @UserName AND Password = @Password;

        -- Set isSuccess to 1
        SET @isSuccess = 1;

        -- Check if the role is 'User' and retrieve UserId
        IF @Role = 'User'
        BEGIN
            SELECT @UserId = Id
            FROM user_table
            WHERE Name = @UserName;
        END
    END
    ELSE
    BEGIN
        -- If login fails, set default values
        SET @Role = NULL;
        SET @isSuccess = 0;
        SET @UserId = 0;
    END
END
-------------------------------------------------------------------------------------------------------------------------
create procedure sp_checkAdminExists
	@Exists bit output
as
begin
	if exists(select 1 from login_table where Category = 'Admin')
	begin
		set @Exists = 1;
	end
	else
	begin
		set @Exists = 0;
	end
end

----------------------------------------------------------------------------------------------------------------------------------------------
create or alter procedure sp_fetchUserId
	@email varchar(30),
	@Id int output
as
begin
	select @Id = Id from login_table where Email = @email;
end
----------------------------------------------------------------------------------------------------------------------------------------------
