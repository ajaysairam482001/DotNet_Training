use Infinite_db;

CREATE TABLE COURSEDETAILS (
    C_id VARCHAR(10) PRIMARY KEY,
    C_Name VARCHAR(26),
    Start__date DATE,
    End__date DATE,
    Fee INT	
);

insert into COURSEDETAILS (C_id, C_Name, Start__date, End__date, Fee)
values
('DV004', 'DataVisualization', '2018-03-01','2018-04-15',15000),
('DN003','DotNet', '2018-02-01','2018-02-28', 15000),
('JA002','AdvancedJava', '2018-01-02', '2018-01-20',10000),
('JC001', 'CoreJava', '2018-01-02','2018-01-12', 3000);

select * from COURSEDETAILS;

--q1 function question below

create function CalculateCourseDuration(@start_date DATE, @end_date DATE)
returns int
as
begin
    return DATEDIFF(DAY, @start_date, @end_date);
end;

SELECT
    C_id,C_Name,Start__date,Fee,
    dbo.CalculateCourseDuration(Start__date, End__date) AS Course_In_days
FROM COURSEDETAILS;

------------------------------------------------------------------------------------------------------------------------------------------

-- q2 same table create a trigger
create table T_CourseInfo (
    CourseName VARCHAR(26),
    StartDate DATE
);
create trigger trg_Inserted_Info
on COURSEDETAILS
after insert
as
begin
    insert into T_CourseInfo (CourseName, StartDate)
    select C_Name, Start__date
    from INSERTED;
end; 
--check the trigger
insert into COURSEDETAILS(C_id, C_Name, Start__date, End__date, Fee) VALUES ('YT701', 'THERMODYNAMICS', '2024-10-14', '2025-01-10', 18700);

SELECT * FROM T_CourseInfo;

------------------------------------------------------------------------------------------------------------------------------------------
--q3
CREATE TABLE Products_Details (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,  
    ProductName VARCHAR(55) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    DiscountedPrice AS (Price - (Price * 0.1)) 
);
select * from Products_Details;
create proc Insert_ProdDetails
    @ProductName VARCHAR(55),
    @Price DECIMAL(10, 2),
    @GeneratedProductId INT OUTPUT,
    @DiscountedPrice DECIMAL(10, 2) OUTPUT
as
begin
    declare @InsertedProducts table (ProductId INT);
    
    INSERT INTO Products_Details (ProductName, Price)
    OUTPUT INSERTED.ProductId INTO @InsertedProducts
    VALUES (@ProductName, @Price);
    
    SELECT @GeneratedProductId = ProductId FROM @InsertedProducts;
    
    SET @DiscountedPrice = @Price - (@Price * 0.1);
END;
 

 
DECLARE @GeneratedProductId INT, @DiscountedPrice DECIMAL(10, 2);

-- Call the procedure (checking)
EXEC Insert_ProdDetails
    @ProductName = 'Iphone15',
    @Price = 50000,
    @GeneratedProductId = @GeneratedProductId OUTPUT,
    @DiscountedPrice = @DiscountedPrice OUTPUT;
SELECT @GeneratedProductId AS ProductId, @DiscountedPrice AS DiscountedPrice
SELECT * FROM Products_Details;