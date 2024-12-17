
--stored Procedures

create or alter procedure sp_checkLogin
	@UserName varchar(30),
	@Password varchar(100),
	@Role varchar(5) output,
	@isSuccess bit output
as
begin
	if exists (select 1 from login_table where UserName = @UserName and Password = @Password)
	begin
	select @Role = Category from login_table where UserName = @UserName and Password = @Password;
	set @isSuccess = 1;
	end
	else
	begin
	set @Role = NuLL;
	set @isSuccess = 0;
	end
end

DECLARE @Role VARCHAR(5), @isSuccess BIT;
EXEC sp_checkLogin @UserName = 'rothih', @Password = '1234', @Role = @Role OUTPUT, @isSuccess = @isSuccess OUTPUT;

SELECT @Role AS Role, @isSuccess AS IsSuccess;
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
