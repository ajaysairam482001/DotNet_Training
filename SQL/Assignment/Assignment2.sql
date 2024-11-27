use Infinite_db;

create table EMP(EmpNo int,Ename varchar(21) not null,Job varchar(20),Mgr_id int,HireDate Date,Sal float, Comm int,DeptNo int
primary key (EmpNo),
foreign key (DeptNo) references DEPT(DeptNo));

create table DEPT(DeptNo int, Dname varchar(25), dLocation varchar(40),
primary key(DeptNo));

select * from EMP;
select * from DEPT;

truncate table EMP;

insert into Dept values(10,'Accounting','New York'),
(20,'Research','Dallas'),
(30,'Sales','Chicago'),
(40,'Operations','Boston');

insert into Emp values(7369,'SMITH','CLERK',7902,'17-Dec-80',800,null,20),
(7499,'ALLEN','SALESMAN',7698,'20-FEB-81',1600,300,30),
(7521,'WARD','SALESMAN',7698,'22-FEB-81',1250,500,30),
(7566,'JONES','MANAGER',7839,'02-APR-81',2975,null,20),
(7654,'MARTIN','SALESMAN',7698,'28-SEP-81',1250,1400,30),
(7698,'BLAKE','MANAGER',7839,'01-MAY-81',2850,null,30),
(7782,'CLARK','MANAGER',7839,'09-JUN-81',2450,null,10),
(7788,'SCOTT','ANALYST',7566,'19-APR-87',3000,null,20),
(7839,'KING','PRESIDENT',null,'17-NOV-81',5000,null,10),
(7844,'TURNER','SALESMAN',7698,'08-SEP-81',1500,0,30),
(7876,'ADAMS','CLERK',7788,'23-MAY-87',1100,null,20),
(7900,'JAMES','CLERK',7698,'03-DEC-81',950,null,30),
(7902,'FORD','ANALYST',7566,'03-DEC-81',3000,null,20),
(7934,'MILLER','CLERK',7782,'23-JAN-82',1300,null,10);


--Question1
select * from EMP where Ename like 'A%';

--Question2
select * from EMP where Mgr_id is null;

--Question3
select Ename, EmpNo, Sal from EMP where Sal between 1200 and 1400;

--Question4
update EMP set Sal = Sal+(Sal*0.10) where exists(select DeptNo from DEPT where Dname = 'Research');  --OR
update EMP set Sal = Sal+(Sal*0.10) where DeptNo = (select DeptNo from DEPT where Dname = 'Research');

--Question5
select COUNT(Job) as No_of_Clerks from EMP where Job = 'CLERK';

--Question6
select Count(Job) as No_of_people, Avg(Sal) as Average_Sal from EMP group by Job;

--Question7
select Max(Sal) as 'highest Salary', Min(Sal) as 'lowest Salary' from EMP;

--Question8
select * from DEPT where Not exists(select * from EMP where EMP.DeptNo = DEPT.DeptNo);

--Question9
select Ename,Sal from EMP where Job = 'Analyst' AND DeptNo = 20 AND Sal > 1200 Order by Ename ASC;

--Question10
select d.Dname,d.DeptNo ,Sum(e.Sal) as Salary from DEPT d join EMP e  on d.DeptNo = e.DeptNo Group By d.Dname,d.DeptNo Order by d.DeptNo;

--Question11
select Ename,Sal from EMP where Ename = 'MILLER' or Ename = 'SMITH'; --OR
select Ename,Sal from EMP where Ename in ('MILLER','SMITH');

--Question12
select Ename from EMP where Ename like 'A%' or Ename like 'M%';  --OR
select Ename from EMP where Ename like '[AM]%';

--Question13
select Ename, Sal*12 as YearlySal from EMP where Ename = 'SMITH';	

--Question14
select Ename, Sal from EMP where Sal Not between 1500 and 2850;

--Question15
select Mgr_id, Count(*) as Employees from EMP where Mgr_id is not null group by Mgr_id having count(*)>2;