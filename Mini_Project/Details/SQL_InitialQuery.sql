create database railway_db;
use master;
use railway_db;

--login table
create table login_table(
	Id int primary key identity,
	UserName varchar(30) not null,
	Password varchar(100) not null,
	Category varchar(5) check (Category in ('Admin','User')) not null,
	Email varchar(30) not null unique
);

--User table
create table  user_table(
	Id int primary key,
	Name varchar(30) not null,
	Email varchar(30) unique not null,
	Contact_number bigint unique,
	foreign key (Id) references login_table(ID)
);

--Trains table
create table train_table(
	Train_Number int primary key,
	Train_Name varchar(30) not null,
	Train_Type varchar(30)
)

--Train Schedule Details Table
create table train_Schedule_Details(
	TrainInstanceId int primary key,
	TrainNo int not null,
	_From varchar(30) not null,
	_To varchar(30) not null,
	Timings Time not null,
	HoursOfJourney int not null,
	DateOfDeparture Date not null,
	Status varchar(30) check (Status in ('Active','Cancelled')) not null,
	foreign key (TrainNo) references train_table(Train_Number)
);

--Ticket Table
create table ticket_table(
	TicketNumber int Primary key Identity,
	User_ID int not null,
	TrainInstanceId int not null,
	Class varchar(10) not null,
	Berth varchar(10) not null,
	SeatNumber int not null,
	BookingDate Date not null,
	Foreign key(User_Id) references user_table(Id) on delete cascade,
	Foreign key(TrainInstanceId) references train_Schedule_Details(TrainInstanceId) on delete cascade
);


--BerthDetails_atInstanceTrain Table
create table BerthDetails_table(
	TrainInstanceNo int not null,
	class varchar(10) not null,
	NumberOfAvailableBerth int not null,
	SeatLimit int not null,
	BookedSeats int not null,
	AvailableSeats int not null,
	primary key (TrainInstanceNo , class),  --composite Primary key
	Foreign key (TrainInstanceNo) references train_Schedule_Details(TrainInstanceId) on delete cascade
);

create table TrainInstancePriceDetails(
	priceId int primary key identity,
	TrainInstanceId int not null,
	class varchar(10) not null,
	Price decimal(10,2) not null,
	foreign key(TrainInstanceId) references train_Schedule_Details(TrainInstanceId)	
);

alter table ticket_table
add price decimal(10,2) not null;

select * from ticket_table;
select * from login_table;
select * from user_table;


truncate table login_table;
delete from user_table;
delete from login_table;
--during booking write a procedure to fetch the respective instance berth price from the table