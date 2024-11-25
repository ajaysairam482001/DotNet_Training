create database Infinite_db;

create table clients(
Client_ID int primary key,
Cname varchar(40) not null,
Address varchar(30),
Email varchar(30) unique,
Business varchar(20) not null,
Phone bigint);

select * from clients;

insert into clients values (1001,'ACME Utilities','Noida','contact@acmeutil.com','Manufacturing',9567880032),
(1002,'Trackon Consultants', 'Mumbai', 'consult@trackon.com', 'Consultant', 8734210090),
(1003,'MoneySaver Distributors', 'Kolkata' ,'save@moneysaver.com'  ,'Reseller',7799886655),
(1004 ,'Lawful Corp' ,'Chennai' ,'justice@lawful.com', 'Professional', 9210342219);

create table Departments(
Deptno int primary key,
D_name varchar(25) not null,
Loc varchar(20)
)

select * from Departments;

insert into Departments values (10,'Design','Pune'),(20,'Development','Pune'),
(30,'Testing','Mumbai'),
(40,'Document', 'Mumbai');

create table Employees(
Emp_no int primary key,
E_name varchar(20) not null,
Job varchar(15),
Salary bigint check (Salary>0),
Deptno int references Departments(Deptno))

insert into Employees values (7001,'Sandeep','Analyst',25000, 10),
(7002 ,'Rajesh' ,'Designer',30000,10),
(7003 ,'Madhav' ,'Developer', 40000 ,20),
(7004 ,'Manoj' ,'Developer' ,40000 ,20),
(7005 ,'Abhay' ,'Designer' ,35000 ,10),
(7006 ,'Uma' ,'Tester' ,30000 ,30),
(7007 ,'Gita' ,'Tech. Writer' ,30000 ,40),
(7008 ,'Priya' ,'Tester' ,35000 ,30),
(7009 ,'Nutan' ,'Developer' ,45000 ,20),
(7010 ,'Smita' ,'Analyst' ,20000 ,10),
(7011 ,'Anand' ,'Project Mgr' ,65000 ,10);

select * from Employees;

create table Projects(
Project_ID int primary key,
Descr varchar(30) not null,
start_Date Date,
Planned_End_Date Date,
Actual_End_date Date,
constraint check_date check(planned_end_date<actual_end_date),
Budget bigint check(Budget > 0),
Client_ID int references Clients(Client_ID)
);

insert into Projects Values (401,'Inventory','01-Apr-11','01-Oct-11' ,'31-Oct-11' ,150000 ,1001),
(402, 'Accounting', '01-Aug-11', '01-Jan-12',null,500000 ,1002),
(403,'Payroll', '01-Oct-11', '31-Dec-11' ,null,75000 ,1003),
(404, 'Contact Mgmt', '01-Nov-11' ,'31-Dec-11',null ,50000, 1004);

select *from Projects;
 
 
create table EmpProjectTasks(
Project_ID int references Projects(Project_ID),
Emp_no int references Employees(Emp_no),
primary key(Project_ID,Emp_no),
Start_Date Date,
End_date Date,
Task varchar(25) not null,
Status Varchar(15) not null
)
 
insert into EmpProjectTasks Values 
(401,7001,' 01-Apr-11', '20-Apr-11', 'System Analysis', 'Completed'),
(401,7002, '21-Apr-11', '30-May-11', 'System Design' ,'Completed'),
(401, 7003, '01-Jun-11', '15-Jul-11', 'Coding' ,'Complete'),
(401, 7004, '18-Jul-11' , '01-Sep-11' ,'Coding', 'Completed'),
(401 ,7006, '03-Sep-11', '15-Sep-11', 'Testing','Completed'),
(401, 7009, '18-Sep-11', '05-Oct-11', 'Code Change', 'Completed'),
(401, 7008, '06-Oct-11' ,'16-Oct-11', 'Testing', 'Completed'),
(401, 7007, '06-Oct-11', '22-Oct-11', 'Documentation', 'Completed'),
(401 ,7011, '22-Oct-11', '31-Oct-11', 'Sign off', 'Completed'),
(402 ,7010, '01-Aug-11', '20-Aug-11' ,'System Analysis' ,'Completed'),
(402 ,7002, '22-Aug-11', '30-Sep-11', 'System Design','Completed'),
(402, 7004, '01-Oct-11',null,'Coding In', 'Progress');
 
select *from EmpProjectTasks;