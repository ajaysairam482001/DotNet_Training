use Infinite_db;

create table Books(Book_ID int,title varchar(24) not null,author varchar(30) not null,isbn bigint,published_date Date,
primary key (Book_ID),
unique (isbn));

insert into Books values(1,'My First SQL Book','Mary Parker',981483029127,'2022-02-22 12:08:17'),
(2,'My Second SQL Book','John Mayer',857300923713,'1972-07-03 09:22:45');

insert into Books values(3,'My third SQL Book','Cary Flint',523120967812,'2022-02-22 12:08:17');

drop table Books;
select * from Books;

--Query Question1
select * from Books where author like '%er';