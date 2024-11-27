use Infinite_db;

create table CostumerQA (Id int, _name varchar(20),age int,_Address varchar(20),Salary float,primary key(Id));
   
insert into CostumerQA values
   (1,'Ramesh',32,'Ahmedabad',2000.00),
  (2,'Khilan',25,'Delhi',1500.00),
  (3,'Kaushik',23,'Kota',2000.00),
  (4,'Chaitali',23,'Mumbai',6500.00),
  (5,'Hardik',23,'Bhopal',8500.00),
  (6,'Komal',22,'MP',4500),
  (7,'Muffy',24,'Indore',10000);

--Question (Display the Name for the customer from above customer table who live 
--in same address which has character o anywhere in addres)
select _name from CostumerQA where _Address like '%o%';

  -------------------------------------------------------------------------------------------------------------------------------------------------

create table Orders (order_ID int, Date Datetime, 
Customer_ID int references CostumerQA(Id), amount float)
--drop table Orders; select * from Orders;

insert into orders values 
(102,'2009-10-08',3,3000),(100,'2009-10-08',3,1500), (101,'2009-11-20',2,1560),(103,'2008-05-20',4,2060)

--Question (Write a query to display the Date,Total no of customer placed order on same Date)
select date ,count(order_ID) No_of_Customers from Orders group by date having count(order_ID)>1;

------------------------------------------------------------------------------------------------------------------------------------------------
create table EmployeeQA (Id int, _name varchar(20),age int,_Address varchar(20),Salary float,primary key(Id));

insert into EmployeeQA values
   (1,'Ramesh',32,'Ahmedabad',2000.00),
  (2,'Khilan',25,'Delhi',1500.00),
  (3,'Kaushik',23,'Kota',2000.00),
  (4,'Chaitali',23,'Mumbai',6500.00),
  (5,'Hardik',23,'Bhopal',8500.00),
  (6,'Komal',22,'MP',null),
  (7,'Muffy',24,'Indore',null);

select * from EmployeeQA;
  
--Display the Names of the Employee in lower case, whose salary is null
select Lower(_name) AS Employee_Name from EmployeeQA where Salary IS NULL;