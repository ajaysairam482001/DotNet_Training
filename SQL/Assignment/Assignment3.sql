use Infinite_db;

select * from EMP;
select * from DEPT;

--Question 1
select * from EMP where Job = 'MANAGER';

--Question 2
select Ename , Sal from EMP where sal > 1000;
 
--Question 3
select Ename, Sal from EMP where Ename Not Like 'JAMES';

--Question 4
select * from EMP where Ename like 'S%';

--Question 5
select Ename from EMP where Ename like '%A%';

--Question 6
select Ename from EMP where Ename like '__L%';

--Question 7
select Ename , Daily_Sal = Sal/30  From EMP where Ename like 'Jones';

--Question 8
--select Ename , Daily_Sal = Sal*12 from EMP;
select Sum(sal) as 'Total_Salary' from EMP;

--Question 9
--select (SUM(Sal)/12) as 'AverageSalary' from EMP;
select AVG(Sal*12) as 'AverageSalary' from EMP;

--Question 10
select Ename, Job, Sal from EMP where Job != 'SALESMAN' OR DeptNo != 30;

--Question 11
select distinct deptNo from EMP;

--Question 12
select Ename , Sal as 'Monthly Salary' from EMP where Sal > 1500 and (deptNo = 10 or deptNo = 30);
--select Ename,Sal,Deptno from EMP where Sal>1500 and Deptno in(10,30)

--Question 13
select Ename,Job,Sal from EMP where (Job like 'MANAGER' or Job like 'ANALYST') and ( sal not in (1000,3000,5000));

--Question 14
select Ename,Sal=sal+sal*0.1,Comm from Emp where Comm>sal

---Question 15
select Ename from EMP where Ename like'%ls%' and (Deptno=30 or Mgr_Id=7782)

--Question 16
select Ename, HireDate, 
       datediff(year, HireDate, getdate()) AS ' Experience'
from emp
where datediff(year, HireDate, getDate()) > 30
  and datediff(year, HireDate, getdate()) < 40;
select count(*) AS 'Total employee'
from Emp
where datediff(year, HireDate, getdate()) > 30
  and datediff(year, HireDate, getDate()) < 40;

--Question 17
select d.Dname as Department_Name, e.Ename as Employee_Name FROM EMP e join Dept d ON e.Deptno = d.Deptno order by  e.Ename desc,d.Dname Asc