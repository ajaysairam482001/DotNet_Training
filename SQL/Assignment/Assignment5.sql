use Infinite_db;

--1 

CREATE OR ALTER PROCEDURE GeneratePayslip
    @empId INT
AS
BEGIN
    DECLARE @Salary FLOAT
    DECLARE @HRA FLOAT
    DECLARE @DA FLOAT
    DECLARE @PF FLOAT
    DECLARE @IT FLOAT
    DECLARE @Deductions FLOAT
    DECLARE @GrossSalary FLOAT
    DECLARE @NetSalary FLOAT
 
    SELECT @Salary = sal
    FROM EMP
    WHERE empno = @empId
 
    SET @HRA = @Salary * 0.10
    SET @DA = @Salary * 0.20
    SET @PF = @Salary * 0.08
    SET @IT = @Salary * 0.05
 
    SET @Deductions = @PF + @IT
    SET @GrossSalary = @Salary + @HRA + @DA
 
    SET @NetSalary = @GrossSalary - @Deductions
 
    PRINT 'Employee Payslip for Employee ID: ' + CAST(@empId AS VARCHAR(10))
    PRINT 'Basic Salary: ' + CAST(@Salary AS VARCHAR(20))
    PRINT 'HRA: ' + CAST(@HRA AS VARCHAR(20))
    PRINT 'DA: ' + CAST(@DA AS VARCHAR(20))
    PRINT 'PF: ' + CAST(@PF AS VARCHAR(20))
    PRINT 'IT: ' + CAST(@IT AS VARCHAR(20))
    PRINT 'Total Deductions: ' + CAST(@Deductions AS VARCHAR(20))
    PRINT 'Gross Salary: ' + CAST(@GrossSalary AS VARCHAR(20))
    PRINT 'Net Salary: ' + CAST(@NetSalary AS VARCHAR(20))
END
 
EXEC GeneratePayslip @empId = 7369
 
SELECT * FROM EMP;



--2.Create a trigger to restrict data manipulation on EMP table during General holidays. Display the error message like “Due to Independence day you cannot manipulate data” or "Due To Diwali", you cannot manipulate" etc
CREATE TABLE Holiday (
    holiday_date DATE PRIMARY KEY,
    holiday_name VARCHAR(100)
)
 truncate table Holiday;
 
INSERT INTO Holiday (holiday_date, holiday_name)
VALUES 
    ('2024-08-15', 'Independence Day'),
    ('2024-01-26', 'Republic Holiday'),
    ('2024-12-25', 'Christmas'),
    ('2025-01-01', 'New Year')
	

select * from Holiday;

create or alter trigger RestrictDMLonHolidays on EMP
Instead of insert , update, delete as 
begin
	declare @isHoliday bit;
	declare @HolidayName varchar(100);

	select top 1 @HolidayName = holiday_name from Holiday where holiday_date = CONVERT(DATE, GETDATE());
	
	if @HolidayName is not null 
	begin
		--RAISERROR('Due to %s, you cannot perform manipulation to data.',16,1,@HolidayName);
		RAISERROR('Due to %s, you cannot perform manipulation to data.', 16, 1, @HolidayName);

	end
	else
    begin
        PRINT 'Data manipulation is allowed on non-holiday dates.';
        UPDATE EMP SET Mgr_id = 0000 WHERE Ename = 'SMITH';
    end
end;

select * from EMP;
DELETE FROM EMP WHERE EmpNo = 7369;