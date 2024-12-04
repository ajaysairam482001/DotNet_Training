use Infinite_db;

select * from EMP;
select * from DEPT;

--1.Write a query to display your birthday( day of week)
select DATENAME(weekday, '2001-08-04')as dayofweek;

-------------------------------------------------------------------------------------------------------------------------------


--2.Write a query to display your age in days
select datediff (day,'2001-08-04',getdate()) as Age_IN_Days;

-------------------------------------------------------------------------------------------------------------------------------


--3 Write a query to display all employees information those who joined before 5 years in the current month
select * from EMP e where e.HireDate<dateAdd(year,-5,getDate()) and month(e.HireDate) = month(getdate());

-------------------------------------------------------------------------------------------------------------------------------


--4 Create table Employee with empno, ename, sal, doj columns or use and perform the following operations in a single transaction
--	a. First insert 3 rows 
--	b. Update the second row sal with 15% increment  
 --       c. Delete first row.
--After completing above all actions, recall the deleted row without losing increment of second row.
BEGIN TRANSACTION;
 
-- Create table
CREATE TABLE EMP1 (
    emp_no INT PRIMARY KEY,
    ename VARCHAR(50),
    sal DECIMAL(10, 2),
    DOJ DATE
);
--a
insert into EMP1 (emp_no, ename, sal, DOJ)
Values 
(1, 'John', 5000, '2015-01-01'),
(2, 'Jane', 6000, '2016-02-01'),
(3, 'Doe', 7000, '2017-03-01');

--b
update EMP1 set sal = sal * 1.15 where emp_no = 2;

--c
begin Transaction;
 
DELETE from EMP1
WHERE emp_no = 1;
Rollback; -- i rolled back maynot be in the o/p pics but i ran delete query and then ran rollback it worked!

select * from EMP1;
truncate table EMP1;

-------------------------------------------------------------------------------------------------------------------------------



--5 Create a user defined function calculate Bonus for all employees of a  given dept using 	following conditions
--	a.     For Deptno 10 employees 15% of sal as bonus.
--	b.     For Deptno 20 employees  20% of sal as bonus
--	c      For Others employees 5%of sal as bonus
create function CalculateBonus (@deptNo INT, @Sal decimal(10, 2))
returns decimal(10, 2)
as
begin
    declare @Bonus decimal(10, 2);
    IF @deptNo = 10
        SET @Bonus = @Sal * 0.15;
    ELSE IF @deptNo = 20
        SET @Bonus = @Sal * 0.20;
    ELSE
        SET @Bonus = @Sal * 0.05;
    RETURN @Bonus;
END;
 
--a. 
SELECT ename, sal, dbo.CalculateBonus(deptno, sal) AS Bonus
FROM EMP
WHERE deptno = 10
 
--b.
SELECT ename, sal, dbo.CalculateBonus(deptno, sal) AS bonus
FROM EMP
WHERE deptno = 20
 
 
--c  
SELECT ename, sal, dbo.CalculateBonus(deptno, sal) AS bonus
FROM EMP
WHERE deptno NOT IN (10, 20)


-------------------------------------------------------------------------------------------------------------------------------


--6. Create a procedure to update the salary of employee by 500 whose dept name 
--is Sales and current salary is below 1500 (use emp table)
create or alter proc updateSal
as
begin
 update e
 set e.sal = e.sal + 500 
 from EMP e
 Join DEPT d ON e.DeptNo = d.Deptno
 where d.Dname = 'Sales' and e.sal < 1500;
end

exec updateSal;