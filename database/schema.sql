CREATE DATABASE RestaurantManagement;
GO

USE RestaurantManagement;
GO

CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL
);

CREATE TABLE MenuItems (
    MenuItemId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1
);

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

CREATE TABLE OrderItems (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    MenuItemId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    CONSTRAINT FK_OrderItems_MenuItems FOREIGN KEY (MenuItemId) REFERENCES MenuItems(MenuItemId)
);
GO

