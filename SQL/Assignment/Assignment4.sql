use Infinite_db;

--find the factorial of a number.
Declare @num int = 5;
declare @fact bigint = 1;
if @num < 0
begin
	print'Factorial is not defined for negative number';
end
else
begin
		while @num>0
		begin
			set @fact = @fact * @num;
			set @num = @num - 1;
		end 
	print 'Factorial of 5 is: ' + cast(@fact as varchar);
end

--creating a stored procedure to take in two values and generate its product table till the mentioned range (2nd input) as output.

create or alter proc multiplication_Table 
@num int,
@range int 
as
begin
	set NoCount on;
	declare @counter int = 1;
	while @counter <= @range
	begin
		print concat(@num ,'x',@counter,'=',@num*@counter);
		set @counter = @counter + 1;
	end
end

exec multiplication_Table @num = 5,@range = 10;

--3 Create a function to calculate the status of the student. If student score >=50 then pass, else fail. Display the data neatly
CREATE TABLE Student (
    Sid INT PRIMARY KEY,
    Sname NVARCHAR(50)
);
 
CREATE TABLE Marks (
    Mid INT PRIMARY KEY,
    Sid INT FOREIGN KEY REFERENCES Student(Sid),
    Score INT
);

INSERT INTO Student (Sid, Sname) VALUES (1, 'Jack');
INSERT INTO Student (Sid, Sname) VALUES (2, 'Rithvik');
INSERT INTO Student (Sid, Sname) VALUES (3, 'Jaspreeth');
INSERT INTO Student (Sid, Sname) VALUES (4, 'Praveen');
INSERT INTO Student (Sid, Sname) VALUES (5, 'Bisa');
INSERT INTO Student (Sid, Sname) VALUES (6, 'Suraj');

INSERT INTO Marks (Mid, Sid, Score) VALUES (1, 1, 23);
INSERT INTO Marks (Mid, Sid, Score) VALUES (2, 6, 95);
INSERT INTO Marks (Mid, Sid, Score) VALUES (3, 4, 98);
INSERT INTO Marks (Mid, Sid, Score) VALUES (4, 2, 17);
INSERT INTO Marks (Mid, Sid, Score) VALUES (5, 3, 53);
INSERT INTO Marks (Mid, Sid, Score) VALUES (6, 5, 13);

 Create function getStudentsStatus(@Score int) returns nvarchar(10) as
 begin
	declare @res nvarchar(10);
	if @score > 50
		set @res = 'Pass';
	else
		set @res = 'Fail';
	return @res;
 end

 select s.Sid as StudentID,
		s.Sname as Name,
		m.Score as score,
		dbo.getStudentsStatus(m.score) AS status
	from 
		Student s
	join 
		Marks m on s.Sid = m.Sid;