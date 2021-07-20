﻿USE GearBatOn
GO

ALTER TABLE AspNetUsers
ADD FullName nvarchar(255);

CREATE TABLE Country
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL DEFAULT N'Chưa đặt tên'
)
INSERT INTO Country VALUES (N'Việt Nam');
INSERT INTO Country VALUES (N'Trung Quốc');
INSERT INTO Country VALUES (N'Đài Loan');
INSERT INTO Country VALUES (N'Mỹ');

CREATE TABLE Province
(
	Id INT IDENTITY PRIMARY KEY,
	CountryId INT NOT NULL,
	Name NVARCHAR(255) NOT NULL DEFAULT N'Chưa đặt tên'
	
	FOREIGN KEY (CountryId) REFERENCES Country(Id),
)
INSERT INTO Province VALUES (1, N'TP Hồ Chí Minh');
INSERT INTO Province VALUES (1, N'Hà Nội');
INSERT INTO Province VALUES (1, N'Đà Nẵng');
INSERT INTO Province VALUES (1, N'Buôn Mê Thuột');

CREATE TABLE Category
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL DEFAULT N'Chưa đặt tên'
)
INSERT INTO Category VALUES (N'Laptop');
INSERT INTO Category VALUES (N'PC');
INSERT INTO Category VALUES (N'CPU');
INSERT INTO Category VALUES (N'Màn hình');

CREATE TABLE Brand
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL DEFAULT N'Chưa đặt tên'
)
INSERT INTO Brand VALUES (N'Intel');
INSERT INTO Brand VALUES (N'AMD');
INSERT INTO Brand VALUES (N'Asus');
INSERT INTO Brand VALUES (N'Acer');
INSERT INTO Brand VALUES (N'Ducky');

CREATE TABLE Product
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL DEFAULT N'Chưa đặt tên',
	CategoryId INT NOT NULL,
	CountryId INT NOT NULL,
	BrandId INT NOT NULL,
	Description NVARCHAR(4000),
	Price MONEY NOT NULL DEFAULT 0,
	WarrantyPeriod INT NOT NULL DEFAULT 0,
	InventoryAmount INT NOT NULL DEFAULT 0,
	ImagePath NVARCHAR(255),
	Status BIT NOT NULL, --False xóa

	FOREIGN KEY (CountryId) REFERENCES Country(Id),
	FOREIGN KEY (CategoryId) REFERENCES Category(Id),
	FOREIGN KEY (BrandId) REFERENCES Brand(Id)
)
INSERT INTO Product VALUES (N'Ryzen Threadripper 3960X', 3, 3, 2, N'Test', '36590000', 36, 100, N'AMD3960x.png', 1);

CREATE TABLE Promotion
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL DEFAULT N'Chưa đặt tên',
	PromoCode NVARCHAR(255) NOT NULL,
	Ratio FLOAT NOT NULL DEFAULT 0,
	Status BIT NOT NULL DEFAULT 1, --False xóa
	CHECK (Ratio >= 0 AND Ratio <= 100)
)
CREATE UNIQUE NONCLUSTERED INDEX IX_Promotion_NonDeletedUnique ON Promotion(PromoCode) WHERE Status=1
INSERT INTO Promotion VALUES (N'Test 30', N'KM30', 30, 1);

CREATE TABLE Invoice
(
	Id INT IDENTITY PRIMARY KEY,
	StaffId nvarchar(128) NOT NULL,
	CustomerId nvarchar(128) NOT NULL DEFAULT 2,
	Date DATETIME NOT NULL DEFAULT GETDATE(),
	PromotionId INT,
	CountryId INT NOT NULL,
	ProvinceId INT NOT NULL,
	Address NVARCHAR(255) NOT NULL,
	PaymentStatus BIT NOT NULL DEFAULT 0, -- 1 đã thanh toán ; 0 chưa thanh toán	
	
	FOREIGN KEY (PromotionId) REFERENCES Promotion(Id),
	FOREIGN KEY (StaffId) REFERENCES AspNetUsers(Id),
	FOREIGN KEY (CustomerId) REFERENCES AspNetUsers(Id),
	FOREIGN KEY (CountryId) REFERENCES Country(Id),
	FOREIGN KEY (ProvinceId) REFERENCES Province(Id)
)

CREATE TABLE InvoiceDetails
(
	Id INT IDENTITY PRIMARY KEY,
	InvoiceId INT NOT NULL,
	ProductId INT NOT NULL,
	Amount INT NOT NULL default 1,
	
	FOREIGN KEY (InvoiceId) REFERENCES Invoice(Id),
	FOREIGN KEY (ProductId) REFERENCES Product(Id)
)

BACKUP DATABASE GearBatOn TO DISK = 'D:\GearBatOn.bak'