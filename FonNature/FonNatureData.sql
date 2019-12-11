SET DATEFORMAT MDY

CREATE Database FonNature
GO -- Thay cho dau cham phay

Use FonNature 
GO


-- TABLE IN DATABASE: 
-- 1.  Position
-- 2.  Staff
-- 3.  Account
-- 4.  Member
-- 5.  Customer
-- 6.  Product Category
-- 7.  Promotion
-- 8.  Supplier
-- 9.  Color
-- 10.  Product
-- 11. Import Invoice
-- 12. Import Invoice Information
-- 13. StatusOrder
-- 14. Orders
-- 15. Order Information
-- 16. Export Invoice

-- AREA CREATE DATABASE
-- 1. POSITION
CREATE TABLE Position
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(100) NOT NULL,
)

--2. STAFF
CREATE TABLE Staff
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(100) NOT NULL,
	Email NVARCHAR(100),
	Address NVARCHAR(100),
	Phone NCHAR(20),
	IdPosition int,

	FOREIGN KEY(IdPosition) REFERENCES Position(Id)
)

--3. ACCOUNT
CREATE TABLE Account
(
	UserName NVARCHAR(100) PRIMARY KEY,
	Password NVARCHAR(100) NOT NULL,	
	DisplayName NVARCHAR(100),
	IdStaff int,

	FOREIGN KEY(IdStaff) REFERENCES Staff(Id)
)

--4. Member
CREATE TABLE Member
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(100) NOT NULL,
)
--5. Customer
CREATE TABLE Customer
(
	Id BIGINT PRIMARY KEY IDENTITY,
	Name NVARCHAR(100) NOT NULL,
	Email NVARCHAR(100),
	Address NVARCHAR(100),
	Phone NCHAR(20),
	IdMember int,

	FOREIGN KEY(IdMember) REFERENCES Member(Id)
)
--6. ProductCategory
CREATE TABLE ProductCategory
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(100) NOT NULL,
)
--7. Promotion
CREATE TABLE Promotion
(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(100) NOT NULL,
	Coupon NCHAR(10) NOT NULL,
	Quantity int NOT NULL,
	PromotionPrice DECIMAL(18,0) NOT NULL,
)
--8. Supplier
CREATE TABLE Supplier
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(100) NOT NULL,
	Address NVARCHAR(100),
	Phone NCHAR(20),
	Email NVARCHAR(100),
	SupplierProduct NVARCHAR(250)
)

-- 9. Color
CREATE TABLE Color
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(100) NOT NULL
)
--10. Product
CREATE TABLE Product
(
	Id NVARCHAR(100) PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	Quantity INT default 0 NOT NULL,
	Price DECIMAL(18,0) default 0  NOT NULL,
	IdCategory INT NOT NULL,
	IdPromotion INT,
	IdSupplier INT NOT NULL,
	IdColor INT NOT NULL
	
	FOREIGN KEY(IdCategory) REFERENCES ProductCategory(Id),
	FOREIGN KEY(IdPromotion) REFERENCES Promotion(Id),
	FOREIGN KEY(IdSupplier) REFERENCES Supplier(Id),
	FOREIGN KEY(IdColor) REFERENCES Color(Id)
)

--11. Import Invoice
CREATE TABLE ImportInvoice
(
	Id BIGINT PRIMARY KEY IDENTITY,
	Date DATE DEFAULT GETDATE(),
	IdSupplier INT NOT NULL,

	FOREIGN KEY(IdSupplier) REFERENCES Supplier(Id)
)
--12. Import Invoice Information
CREATE TABLE ImportInvoiceInformation
(
	IdImportInvoice BIGINT,
	IdProduct NVARCHAR(100),
	Quantity INT NOT NULL,
	Price DECIMAL(18,0) NOT NULL

	PRIMARY KEY(IdImportInvoice,IdProduct),
	FOREIGN KEY(IdImportInvoice) REFERENCES ImportInvoice(Id),
	FOREIGN KEY(IdProduct) REFERENCES Product(Id)
)

--13. Status Order
CREATE TABLE StatusOrder
(
	Id int PRIMARY KEY IDENTITY,
	Name NVARCHAR(100)
)

--14. Orders
CREATE TABLE Orders
(
	Id BIGINT PRIMARY KEY IDENTITY,
	IdCustomer BIGINT NOT NULL,
	IdStatus INT NOT NULL,
	TotalPrice DECIMAL(18,0),
	Date date default Getdate()
	

	FOREIGN KEY(IdCustomer) REFERENCES Customer(Id),
	FOREIGN KEY(IdStatus) REFERENCES StatusOrder(Id)
)

-- 15. Order Information
CREATE TABLE OrderInformation
(
	IdOrder BIGINT,
	IdProduct NVARCHAR(100) NOT NULL,
	Quantity INT NOT NULL,
	Price DECIMAL(18,0)

	PRIMARY KEY(IdOrder,IdProduct)
	FOREIGN KEY(IdOrder) REFERENCES Orders(Id),
	FOREIGN KEY(IdProduct) REFERENCES Product(Id)
)

-- 16. Export Invoice
CREATE TABLE ExportInvoice
(
	Id nvarchar(100) PRIMARY KEY,
	IdOrder BIGINT,
	Date DATE DEFAULT GETDATE(),

	FOREIGN KEY(IdOrder) REFERENCES Orders(Id)
)

-- AREA INSERT NECESSARY INFORMATION
-- Insert into Position
INSERT INTO Position (Name)
	VALUES (N'Manager');
INSERT INTO Position (Name)
	VALUES (N'CEO');
INSERT INTO Position (Name)
	VALUES (N'Sales');

-- Insert Into Staff
INSERT INTO Staff (Name, Email, Address, Phone, IdPosition)
	VALUES (N'Lê Minh Phước', N'kevinle021097@gmail.com', N'83 Hoàng Hoa Thám', N'0978373449', 2);
INSERT INTO Staff (Name, Email, Address, Phone, IdPosition)
	VALUES (N'Lê Minh Lộc', N'kevinle02@gmail.com', N'83 Hoàng Hoa Thám', N'0978373449', 3);
INSERT INTO Staff (Name, Email, Address, Phone, IdPosition)
	VALUES (N'Phạm Thùy Vân', N'kevinle02@gmail.com', N'83 Hoàng Hoa Thám', N'0978373449', 1);

-- Insert Into Account
INSERT INTO Account (UserName, Password, DisplayName, IdStaff)
	VALUES (N'admin', N'81dc9bdb52d04dc20036dbd8313ed055', N'CEO', 1);
INSERT INTO Account (UserName, Password, DisplayName, IdStaff)
	VALUES (N'manager', N'81dc9bdb52d04dc20036dbd8313ed055', N'Manager', 3);
INSERT INTO Account (UserName, Password, DisplayName, IdStaff)
	VALUES (N'staff', N'81dc9bdb52d04dc20036dbd8313ed055', N'STAFF', 2);

-- Insert Into Member
INSERT INTO Member (Name)
	VALUES (N'Thường');
INSERT INTO Member (Name)
	VALUES (N'Bạc');
INSERT INTO Member (Name)
	VALUES (N'Vàng');
INSERT INTO Member (Name)
	VALUES (N'Kim Cương');

-- Insert Into Status Order
INSERT INTO StatusOrder (Name)
	VALUES (N'Chưa giao hàng');
INSERT INTO StatusOrder (Name)
	VALUES (N'Đã giao hàng cho chuyển phát nhanh');
INSERT INTO StatusOrder (Name)
	VALUES (N'Đã hoàn thành');
GO

-- STORED PROCEDURES

-- 1.  Position
--Get List
CREATE PROC SP_Position_GetList
AS
BEGIN
	SELECT * FROM Position;
END
GO
--Add
CREATE PROC SP_Position_Add
@Name NVARCHAR(100)
AS
BEGIN
	INSERT INTO Position (Name)
	VALUES (@Name);
END

GO
--Update
CREATE PROC SP_Position_Update
@Id int, @Name NVARCHAR(100)
AS
BEGIN
	Update Position
	set
	Name = @Name where Id = @Id
END

EXEC SP_Position_Update @Id = 2, @Name = 'CEO'
SELECT * from Position

GO

--Delete
CREATE PROC SP_Position_Del
@Id int
AS
BEGIN
	Delete from Position
	where Id = @Id
END

GO

-- SearchByName
alter PROC SP_Position_SearchByName
@Name nvarchar(100)
AS
BEGIN
	Select * from Position where Name LIKE '%' + @Name + '%' 
END

EXEC SP_Position_SearchByName @Name = 'o'

-- 2.  Staff
--Get List
CREATE PROC SP_Staff_GetList
AS
BEGIN
	SELECT * FROM Staff;
END
GO
--Add
CREATE PROC SP_Staff_Add
@Name NVARCHAR(100), @Email NVARCHAR(100), @Address NVARCHAR(100), @Phone NVARCHAR(20), @idPosition int
AS
BEGIN
	INSERT INTO Staff(Name, Email, Address, Phone, IdPosition)
	VALUES (@Name, @Email, @Address, @Phone, @idPosition);
END

GO
--Update
CREATE PROC SP_Staff_Update
@Id int, @Name NVARCHAR(100), @Email NVARCHAR(100), @Address NVARCHAR(100), @Phone NVARCHAR(20), @idPosition int
AS
BEGIN
	Update Staff
	Set 
	Name = @Name, Email = @Email,
	Address = @Address, Phone = @Phone, IdPosition = @idPosition
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Staff_Del
@Id int
AS
BEGIN
	Delete from Staff
	where Id = @Id
END

GO

-- SearchByName
CREATE PROC SP_Staff_SearchByName
@Name nvarchar(100)
AS
BEGIN
	Select * from Staff where Name LIKE '%' + @Name + '%' 
END

EXEC SP_Staff_SearchByName @Name = 'p'

-- 3.  Account
-- Get Detail
CREATE PROC SP_Account_GetDetail
@userName NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Account a WHERE a.UserName = @userName;
END

EXEC SP_Account_GetDetail @userName = N'admin'
GO

--Get List
CREATE PROC SP_Account_GetList
AS
BEGIN
	SELECT * FROM Account a;
END
GO

--Add
CREATE PROC SP_Account_Add
@userName NVARCHAR(100), @passWord NVARCHAR(100), @displayName NVARCHAR(100), @idStaff int
AS
BEGIN
	INSERT INTO Account (UserName, Password, DisplayName, IdStaff)
	VALUES (@userName, @passWord, @displayName, @idStaff);
END

GO

-- Edit
CREATE PROC SP_Account_Edit
@userName NVARCHAR(100),@passWord NVARCHAR(100), @displayName NVARCHAR(100), @idStaff int
AS
BEGIN
	UPDATE Account 
	SET 
	Password = @passWord
   ,DisplayName = @displayName
   ,IdStaff = @idStaff
	WHERE UserName = @userName;
END
GO

-- Delete
CREATE PROC SP_Account_Delete
@userName NVARCHAR(100)
AS
BEGIN
	DELETE FROM dbo.Account
    WHERE UserName = @userName
END
GO

--Search
CREATE PROC SP_Account_SearchByName
@userName NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Account a WHERE a.UserName LIKE '%'+@userName+'%'
END

EXEC SP_Account_SearchByName @userName = N'm'
GO

-- List Account by Position
CREATE proc SP_Account_ByPosition
@position nvarchar(100)
as
begin
	SELECT * FROM Account a, Staff s, Position p WHERE a.IdStaff = s.Id and s.IdPosition = p.Id and p.Name = @position
end

EXEC SP_Account_ByPosition	@position = 'Manager'

--Login
CREATE PROC SP_Account_Login
@userName NVARCHAR(100), @passWord NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Account a WHERE a.UserName = @userName AND a.Password = @passWord
END


-- Check Exits
CREATE proc SP_Account_CheckExits
@exits bit OUTPUT,
@userName nvarchar(100)
as
begin
DECLARE @count int
	SELECT @count = count(*) 
	FROM Account a 
	WHERE a.UserName = @userName
end
BEGIN
 if(@count > 0)
 set @exits = 0;
 else
 set @exits = 1;
 end
 GO


-- 4.  Member
--Get List
CREATE PROC SP_Member_GetList
AS
BEGIN
	SELECT * FROM Member;
END
GO
--Add
CREATE PROC SP_Member_Add
@Name NVARCHAR(100)
AS
BEGIN
	INSERT INTO Member (Name)
	VALUES (@Name);
END

GO
--Update
CREATE PROC SP_Member_Update
@Id int, @Name NVARCHAR(100)
AS
BEGIN
	Update Member
	set Name = @Name
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Member_Del
@Id int
AS
BEGIN
	Delete from Member
	where Id = @Id
END

GO


-- 5.  Customer
--Get List
CREATE PROC SP_Customer_GetList
AS
BEGIN
	SELECT * FROM Customer;
END
GO
--Add
create PROC SP_Customer_Add
@Id bigint out,
@Name NVARCHAR(100), @Email NVARCHAR(100), @Address NVARCHAR(100), @Phone NVARCHAR(20), @idMember int
AS
BEGIN
	INSERT INTO Customer (Name, Email, Address, Phone, IdMember)
	VALUES (@Name, @Email, @Address, @Phone, @idMember);
	SELECT @Id = SCOPE_IDENTITY();
END

GO


--Update 
CREATE PROC SP_Customer_Update
@Id BIGINT, @Name NVARCHAR(100), @Email NVARCHAR(100), @Address NVARCHAR(100), @Phone NVARCHAR(20), @idMember int
AS
BEGIN
	Update Customer
	set 
	Name = @Name, Email = @Email, 
	Address = @Address, Phone = @Phone, IdMember = @idMember
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Customer_Del
@Id BIGINT
AS
BEGIN
	Delete from Customer
	where Id = @Id
END

GO

--Search By Name
--Search
CREATE PROC SP_Customer_SearchByName
@Name NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Customer a WHERE a.Name LIKE '%'+@Name+'%'
END
GO

--Search By Phone
--Search
CREATE PROC SP_Customer_SearchByPhone
@Phone NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Customer a WHERE a.Phone LIKE '%'+@Phone+'%'
END






-- 6.  Product Category
--Get List
CREATE PROC SP_Product_Category_GetList
AS
BEGIN
	SELECT * FROM ProductCategory;
END
GO
--Add
CREATE PROC SP_Product_Category_Add
@Name NVARCHAR(100)
AS
BEGIN
	INSERT INTO ProductCategory (Name)
	VALUES (@Name);
END

GO
--Update
CREATE PROC SP_Product_Category_Update
@Id int, @Name NVARCHAR(100)
AS
BEGIN
	Update ProductCategory
	set Name = @Name 
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Product_Category_Del
@Id int
AS
BEGIN
	Delete from ProductCategory
	where Id = @Id
END

GO

--Search By Name
--Search
CREATE PROC SP_ProductCategory_SearchByName
@Name NVARCHAR(100)
AS
BEGIN
	SELECT * FROM ProductCategory a WHERE a.Name LIKE '%'+@Name+'%'
END




-- 7.  Promotion
--Get List
CREATE PROC SP_Promotion_GetList
AS
BEGIN
	SELECT * FROM Promotion;
END
GO
--Add
CREATE PROC SP_Promotion_Add
@Title NVARCHAR(100), @Coupon NCHAR(10), @Quantity int, @Price DECIMAL(18,0)
AS
BEGIN
	INSERT INTO Promotion (Title, Coupon, Quantity, PromotionPrice)
	VALUES (@Title, @Coupon, @Quantity, @Price);
END

GO
--Update
CREATE PROC SP_Promotion_Update
@Id int, @Title NVARCHAR(100), @Coupon NCHAR(10), @Quantity int, @Price DECIMAL(18,0)
AS
BEGIN
	Update Promotion
	set 
	Title = @Title, Coupon = @Coupon, Quantity = @Quantity, PromotionPrice = @Price
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Promotion_Del
@Id int
AS
BEGIN
	Delete from Promotion
	where Id = @Id
END

GO
--Search By Name
--Search
CREATE PROC SP_Promotion_SearchByName
@Name NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Promotion a WHERE a.Title LIKE '%'+@Name+'%'
END
GO

--Search By Coupon
--Search
CREATE PROC SP_Promotion_SearchByCoupon
@Name NVARCHAR(10)
AS
BEGIN
	SELECT * FROM Promotion a WHERE a.Coupon LIKE '%'+@Name+'%'
END
GO


-- Check Exits Coupon
CREATE proc SP_Promotion_CheckExits
@exits bit OUTPUT,
@coupon nvarchar(10)
as
begin
DECLARE @count int
	SELECT @count = count(*) 
	FROM Promotion a 
	WHERE a.Coupon = @coupon
end
BEGIN
 if(@count > 0)
 set @exits = 0;
 else
 set @exits = 1;
 end
 GO



-- 8.  Supplier
--Get List
CREATE PROC SP_Supplier_GetList
AS
BEGIN
	SELECT * FROM Supplier;
END
GO
--Add
CREATE PROC SP_Supplier_Add
@Name NVARCHAR(100),  @Address NVARCHAR(100), @Phone NVARCHAR(20), @Email NVARCHAR(100), @SupplierProduct NVARCHAR(250)
AS
BEGIN
	INSERT INTO Supplier(Name, Address, Phone, Email, SupplierProduct)
	VALUES (@Name, @Address, @Phone, @Email, @SupplierProduct);
END

GO
--Update 
CREATE PROC SP_Supplier_Update
@Id int, @Name NVARCHAR(100), @Address NVARCHAR(100), @Phone NVARCHAR(20), @Email NVARCHAR(100), @SupplierProduct NVARCHAR(250)
AS
BEGIN
	Update Supplier
	set
	Name = @Name, Address = @Address,
	Phone = @Phone, Email = @Email, SupplierProduct = @SupplierProduct
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Supplier_Del
@Id int
AS
BEGIN
	Delete from Supplier
	where Id = @Id
END

GO
--Search By Name
--Search
CREATE PROC SP_Supplier_SearchByName
@Name NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Supplier a WHERE a.Name LIKE '%'+@Name+'%'
END




-- 9.  Color
--Get List
CREATE PROC SP_Color_GetList
AS
BEGIN
	SELECT * FROM Color;
END
GO
--Add
CREATE PROC SP_Color_Add
@Name NVARCHAR(100)
AS
BEGIN
	INSERT INTO Color(Name)
	VALUES (@Name);
END

GO
--Update 
CREATE PROC SP_Color_Update
@Id int, @Name NVARCHAR(100)
as
BEGIN
	Update Color
	set
	Name = @Name
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Color_Del
@Id int
AS
BEGIN
	Delete from Color
	where Id = @Id
END

GO

-- 10.  Product
--Get List
CREATE PROC SP_Product_GetList
AS
BEGIN
	SELECT * FROM Product;
END
GO
--Add
create PROC SP_Product_Add
@Id nvarchar(100), @Name NVARCHAR(100), 
@Price DECIMAL(18,0), @IdCategory int, @IdPromotion int = Null , @IdSupplier INT, @IdColor int
AS
BEGIN
	INSERT INTO Product (Id ,Name, Price, IdCategory, IdPromotion, IdSupplier, IdColor)
	VALUES (@Id, @Name, @Price, @IdCategory, @IdPromotion, @IdSupplier, @IdColor);
END
GO

--Update
create PROC SP_Product_Update
@Id NVARCHAR(100), @Name NVARCHAR(100), 
@Price DECIMAL(18,0), @IdCategory int, @IdPromotion int, @IdSupplier int, @IdColor int
AS
BEGIN
	Update Product
	set
	Name = @Name,
	Price = @Price, IdCategory = @IdCategory, IdPromotion = @IdPromotion, IdSupplier = @IdSupplier, IdColor = @IdColor
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Product_Del
@Id NVARCHAR(100)
AS
BEGIN
	Delete from Product
	where Id = @Id
END
GO

-- Get List Color
ALTER PROC SP_Product_ListColor
@Name NVARCHAR(100)
AS
BEGIN
	SELECT c.Name, c.Id FROM Product p, Color c
	where  p.IdColor = c.Id AND p.Name = @Name
END

GO

-- Check Exits Id
CREATE proc SP_Product_CheckExits
@exits bit OUTPUT,
@Id nvarchar(100)
as
begin
DECLARE @count int
	SELECT @count = count(*) 
	FROM Product a 
	WHERE a.Id = @Id
end
BEGIN
 if(@count > 0)
 set @exits = 0;
 else
 set @exits = 1;
 end
 GO

 --Search By Name
--Search
CREATE PROC SP_Product_SearchByName
@Name NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Product a WHERE a.Name LIKE '%'+@Name+'%'
END




-- 11. Import Invoice
--Get List
CREATE PROC SP_Import_Invoice_GetList
AS
BEGIN
	SELECT * FROM ImportInvoice;
END
GO
--Add
CREATE PROC SP_Import_Invoice_Add
@Id bigint out,
@Date DATE, @IdSupplier int
AS
BEGIN
BEGIN TRY
          BEGIN TRANSACTION
	INSERT INTO ImportInvoice (Date, IdSupplier)
	VALUES (@Date, @IdSupplier);
	SELECT @Id = SCOPE_IDENTITY();
	COMMIT TRANSACTION
			print 'transaction committed';
          END TRY

          BEGIN CATCH
          	PRINT  'error when inserting, rolling back transaction';
			rollback tran;
          END CATCH;
END
GO


GO
--Update 
CREATE PROC SP_Import_Invoice_Update
@Id BIGINT, @Date DATE, @IdSupplier int
AS
BEGIN
	Update ImportInvoice
	set
	Date = @Date, IdSupplier = @IdSupplier
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Import_Invoice_Del
@Id BIGINT
AS
BEGIN
	Delete from ImportInvoice
	where Id = @Id
END

GO

create PROC SP_Import_Invoice_GetByDate
@startDay date, @endDay date 
AS
BEGIN
	Select * from ImportInvoice where Date >= @startDay and Date <= @endDay
END
go



--List ImportInvoice by date and page
create PROC SP_Import_Invoice_GetByDateAndPage
@startDay date , @endDay DATE , @page INT
AS
BEGIN
	DECLARE @countRows INT = 10 -- Số lượng dòng mỗi page
	DECLARE @selectedrows INT = @countRows * @page  -- Số tổng dòng tất cả trang trước trang đang chọn, ví dụ lấy trang 3, -> Lấy tổng dòng trang 1 2 3
	DECLARE @exceptrows INT = (@page-1)*@countRows -- Số tổng dòng trang trước trang đang trọn, ví dụ lấy trang 3 -> Lấy tổng dòng trang  1 2

	; WITH ImportInvoiceByDate AS (
	Select * from ImportInvoice where Date >= @startDay and Date <= @endDay
	) 
	SELECT TOP (@selectedrows) * FROM ImportInvoiceByDate 
	EXCEPT
	SELECT TOP (@exceptrows) * FROM ImportInvoiceByDate	
END
GO

exec SP_Import_Invoice_GetByDateAndPage @startDay = '2019-12-06' , @endDay = '2019-12-09' , @page = 1

-- 12. Import Invoice Information
--Get List
CREATE PROC SP_Import_Invoice_Information_GetList
AS
BEGIN
	SELECT * FROM ImportInvoiceInformation;
END
GO
--Add
CREATE PROC SP_Import_Invoice_Information_Add
@IdImportInvoice BIGINT, @IdProduct NVARCHAR(100), @Quantity int, @Price DECIMAL(18,0)
AS
BEGIN
BEGIN TRY
          BEGIN TRANSACTION
	INSERT INTO ImportInvoiceInformation(IdImportInvoice, IdProduct, Quantity, Price)
	VALUES (@IdImportInvoice, @IdProduct, @Quantity, @Price);
	COMMIT TRANSACTION
			print 'transaction committed';
          END TRY

          BEGIN CATCH
          	PRINT  'error when inserting, rolling back transaction';
			rollback tran;
          END CATCH;
END

GO
--Update
CREATE PROC SP_Import_Invoice_Information_Update
@IdImportInvoice BIGINT, @IdProduct NVARCHAR(100), @Quantity int, @Price DECIMAL(18,0)
AS
BEGIN
	Update ImportInvoiceInformation
	set
	Quantity = @Quantity, Price = @Price
	where IdImportInvoice = @IdImportInvoice and IdProduct = @IdProduct
END

GO
--Delete
CREATE PROC SP_Import_Invoice_Information_Del
@IdImportInvoice BIGINT, @IdProduct NVARCHAR(100)
AS
BEGIN
	Delete from ImportInvoiceInformation
	where IdImportInvoice = @IdImportInvoice and IdProduct = @IdProduct
END

GO

--List InvoiceInfo by Id
CREATE PROC SP_Import_Invoice_Information_GetListByIdInvoice
@IdImportInvoice BIGINT
AS
BEGIN
	select * from ImportInvoiceInformation where IdImportInvoice = @IdImportInvoice;
END
GO




-- 13. StatusOrder
--Get List
CREATE PROC SP_Status_Order_GetList
AS
BEGIN
	SELECT * FROM StatusOrder;
END
GO
--Add
CREATE PROC SP_Status_Order_Add
@Name NVARCHAR(100)
AS
BEGIN
	INSERT INTO StatusOrder (Name)
	VALUES (@Name);
END

GO
--Update
CREATE PROC SP_Status_Order_Update
@Id int, @Name NVARCHAR(100)
AS
BEGIN
	Update StatusOrder
	set Name = @Name
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Status_Order_Del
@Id int
AS
BEGIN
	Delete from StatusOrder
	where Id = @Id
END

GO


-- 14. Orders
--Get List
CREATE PROC SP_Order_GetList
AS
BEGIN
	SELECT * FROM Orders;
END
GO
--Add
create PROC SP_Order_Add
@Id bigint out,
@IdCustomer BIGINT, @IdStatus int, @Total DECIMAL(18,0)
AS
BEGIN
 BEGIN TRY
          BEGIN TRANSACTION
	INSERT INTO Orders (IdCustomer, IdStatus, TotalPrice)
	VALUES (@IdCustomer, @IdStatus, @Total);
	SELECT @Id = SCOPE_IDENTITY();
	COMMIT TRANSACTION
			print 'transaction committed';
          END TRY

          BEGIN CATCH
          	PRINT  'error when inserting, rolling back transaction';
			rollback tran;
          END CATCH;
END

GO

--Update
CREATE PROC SP_Order_Update
@Id BIGINT, @IdCustomer BIGINT, @IdStatus int, @Total DECIMAL(18,0)
AS
BEGIN
	Update Orders
	set
	IdCustomer = @IdCustomer, IdStatus = @IdStatus, TotalPrice = @Total
	where Id = @Id
END

GO
--Delete
CREATE PROC SP_Order_Del
@Id BIGINT
AS
BEGIN
	Delete from Orders
	where Id = @Id
END

GO

CREATE Proc SP_Order_GetByDateAndPage
@startDay date , @endDay DATE , @page INT
AS
BEGIN
	DECLARE @countRows INT = 10 -- Số lượng dòng mỗi page
	DECLARE @selectedrows INT = @countRows * @page  -- Số tổng dòng tất cả trang trước trang đang chọn, ví dụ lấy trang 3, -> Lấy tổng dòng trang 1 2 3
	DECLARE @exceptrows INT = (@page-1)*@countRows -- Số tổng dòng trang trước trang đang trọn, ví dụ lấy trang 3 -> Lấy tổng dòng trang  1 2

	; WITH orderByDate AS (
	Select * from Orders where Date >= @startDay and Date <= @endDay
	) 
	SELECT TOP (@selectedrows) * FROM orderByDate 
	EXCEPT
	SELECT TOP (@exceptrows) * FROM orderByDate	
END
GO

create PROC SP_Order_GetByDate
@startDay date, @endDay date 
AS
BEGIN
	Select * from Orders where Date >= @startDay and Date <= @endDay
END
go


--Update
CREATE PROC SP_Order_UpdateStatus
@Id BIGINT, @IdStatus int
AS
BEGIN
	Update Orders
	set
	IdStatus = @IdStatus
	where Id = @Id
END

GO

-- 15. Order Information
--Get List
CREATE PROC SP_Order_Information_GetList
AS
BEGIN
	SELECT * FROM OrderInformation;
END
GO
--Add
create PROC SP_Order_Information_Add
@IdOrder BIGINT, @IdProduct NVARCHAR(100), @Quantity int, @Price DECIMAL(18,0)
AS
BEGIN
BEGIN TRY
          BEGIN TRANSACTION --set transaction isolation level repeatable read;
	INSERT INTO OrderInformation (IdOrder, IdProduct, Quantity, Price)
	VALUES (@IdOrder, @IdProduct, @Quantity, @Price);
	COMMIT TRANSACTION
			print 'transaction committed';
          END TRY

          BEGIN CATCH
          	PRINT  'error when inserting, rolling back transaction';
			rollback tran;
          END CATCH;
END

GO
--Update
CREATE PROC SP_Order_Information_Update
@IdOrder BIGINT, @IdProduct NVARCHAR(100), @Quantity int, @Price DECIMAL(18,0)
AS
BEGIN
	Update OrderInformation
	set
	Quantity = @Quantity, Price = @Price
	where IdOrder = @IdOrder and IdProduct = @IdProduct
END

GO
--Delete
CREATE PROC SP_Order_Information_Del
@IdOrder BIGINT, @IdProduct NVARCHAR(100)
AS
BEGIN
	delete from OrderInformation
	where IdOrder = @IdOrder and IdProduct = @IdProduct
END

GO

--List InvoiceInfo by Id
CREATE PROC SP_Order_Information_GetListByIdOrder
@Id BIGINT
AS
BEGIN
	select * from OrderInformation where IdOrder = @Id;
END
GO




-- 16. Export Invoice
--Get List
CREATE PROC SP_Export_Invoice_GetList
AS
BEGIN
	SELECT * FROM ExportInvoice; 
END
GO
--Add
create PROC SP_Export_Invoice_Add
@Id nvarchar(50), @IdOrder BIGINT, @Date DATE
AS
BEGIN
	INSERT INTO ExportInvoice (Id, IdOrder, Date)
	VALUES (@Id, @IdOrder, @Date);
END

GO
--Update
create PROC SP_Export_Invoice_Update
@Id nvarchar(50), @IdOrder BIGINT, @Date DATE
AS
BEGIN
	Update ExportInvoice
	set 
	IdOrder = @IdOrder, Date = @Date
	where Id = @Id
END

GO
--Delete
create PROC SP_Export_Invoice_Del
@Id nvarchar(50)
AS
BEGIN
	delete from ExportInvoice
	where Id = @Id
END

GO


-- Trigger
alter trigger QuantityPlus
on ImportInvoiceInformation
after insert
as 
begin
declare @InsertedQuantity int
select @InsertedQuantity = Quantity
from inserted
update Product
set Quantity = Quantity + @InsertedQuantity
where id = (select Product.Id 
				from Product join inserted
				on Product.Id = inserted.IdProduct)
end;

GO

create trigger QuantityMinusOrder
on OrderInformation
after insert
as 
begin
declare @InsertedQuantity int
select @InsertedQuantity = Quantity
from inserted
update Product
set Quantity = Quantity - @InsertedQuantity
where id = (select Product.Id 
				from Product join inserted
				on Product.Id = inserted.IdProduct)
end;
go


create trigger DeleteOrderInformation
on OrderInformation
after delete
as 
begin
declare @DeletedQuantity int
select @DeletedQuantity = Quantity
from deleted
update Product
set Quantity = Quantity + @DeletedQuantity
where id = (select Product.Id 
				from Product join deleted
				on Product.Id = deleted.IdProduct)
end;
go

create trigger DeleteImportInformation
on ImportInvoiceInformation
after delete
as 
begin
declare @DeletedQuantity int
select @DeletedQuantity = Quantity
from deleted
update Product
set Quantity = Quantity - @DeletedQuantity
where id = (select Product.Id 
				from Product join deleted
				on Product.Id = deleted.IdProduct)
end;