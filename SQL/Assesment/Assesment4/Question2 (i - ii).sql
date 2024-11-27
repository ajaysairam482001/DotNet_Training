use Infinite_db;

create table Reviews(R_id int, book_Id int, reviewer_Name varchar(20) not null,content varchar(40),rating int, published_date Date,
primary key (R_id),
foreign key(book_id) references Books(Book_ID));

select * from Reviews;
select * from Books;

insert into Reviews values(1,1,'John Smith','My first Review',4,'2017-12-10 05:15:11'),
(2,2,'John Smith','My Second Review',5,'2017-10-13 15:05:12'),(3,2,'Alice Walker','Another Review',1,'2017-12-22 23:47:10');

--Question2
select b.title,b.author,r.reviewer_Name from Books b Join Reviews r on b.Book_Id = r.book_Id;

--Question3
select reviewer_Name from Reviews group by reviewer_Name having COUNT(book_Id)>1;