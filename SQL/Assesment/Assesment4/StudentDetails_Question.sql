use Infinite_db;

create table Student_details (reg_no int, _name varchar(20) not null, age int, Qualification varchar(15), mobile_num bigint, Mail_id varchar(30), 
Location varchar(20), Gender varchar(2)
primary key (reg_no),unique (mobile_num,Mail_id));

select * from Student_details;

insert into Student_details values 
(2,'Sai',22,'B.E',9952836777,'Sai@gmail.com','Chennai','M'), 
(3,'Kumar',20,'BSC',7890125648,'Kumar@gmail.com','Madurai','M'), 
(4,'Selvi',22,'B.Tech',8904567342,'Selvi@gmail.com','Selam','F'), 
(5,'Nisha',25,'M.E',7834672310,'Nisha@gmail.com','Theni','F'), 
(6,'SaiSaran',21,'B.A',7890345678,'Saran@gmail.com','Madurai','F'), 
(7,'Tom',23,'BCA',8901234675,'Tom@gmail.com','Pune','M')

--Write a sql server query to display the Gender,Total no of male and female from the above relation
select Gender, Count(Gender) _count from Student_details group by Gender;
