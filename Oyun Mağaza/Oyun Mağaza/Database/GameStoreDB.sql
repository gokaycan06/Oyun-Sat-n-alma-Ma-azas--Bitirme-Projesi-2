-- Veri tabanını oluştur
CREATE DATABASE GameStoreDB
GO

USE GameStoreDB
GO

-- Users tablosu
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    LastLoginDate DATETIME,
    ProfilePicture NVARCHAR(255),
    WalletBalance DECIMAL(10,2) DEFAULT 0.00,
    IsActive BIT DEFAULT 1
)

-- Developers tablosu
CREATE TABLE Developers (
    DeveloperID INT PRIMARY KEY IDENTITY(1,1),
    DeveloperName NVARCHAR(100) NOT NULL,
    Description NTEXT,
    FoundedYear INT,
    Website NVARCHAR(255)
)

-- Publishers tablosu
CREATE TABLE Publishers (
    PublisherID INT PRIMARY KEY IDENTITY(1,1),
    PublisherName NVARCHAR(100) NOT NULL,
    Description NTEXT,
    FoundedYear INT,
    Website NVARCHAR(255)
)

-- Categories tablosu
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255)
)

-- Games tablosu
CREATE TABLE Games (
    GameID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    Description NTEXT,
    DeveloperID INT FOREIGN KEY REFERENCES Developers(DeveloperID),
    PublisherID INT FOREIGN KEY REFERENCES Publishers(PublisherID),
    ReleaseDate DATE,
    Price DECIMAL(10,2) NOT NULL,
    DiscountPercentage DECIMAL(5,2) DEFAULT 0.00,
    AgeRating NVARCHAR(10),
    SystemRequirements NTEXT,
    CoverImage NVARCHAR(255),
    TrailerUrl NVARCHAR(255)
)

-- GameCategories tablosu (Çoka-çok ilişki)
CREATE TABLE GameCategories (
    GameID INT FOREIGN KEY REFERENCES Games(GameID),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    PRIMARY KEY (GameID, CategoryID)
)

-- UserLibrary tablosu
CREATE TABLE UserLibrary (
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    GameID INT FOREIGN KEY REFERENCES Games(GameID),
    PurchaseDate DATETIME DEFAULT GETDATE(),
    PlayTime INT DEFAULT 0,
    LastPlayed DATETIME,
    PRIMARY KEY (UserID, GameID)
)

-- Purchases tablosu
CREATE TABLE Purchases (
    PurchaseID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    GameID INT FOREIGN KEY REFERENCES Games(GameID),
    PurchaseDate DATETIME DEFAULT GETDATE(),
    Price DECIMAL(10,2) NOT NULL,
    PaymentMethod NVARCHAR(50),
    TransactionStatus NVARCHAR(20)
)

-- Reviews tablosu
CREATE TABLE Reviews (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    GameID INT FOREIGN KEY REFERENCES Games(GameID),
    Rating INT CHECK (Rating >= 1 AND Rating <= 5),
    ReviewText NTEXT,
    ReviewDate DATETIME DEFAULT GETDATE(),
    HelpfulCount INT DEFAULT 0
)

-- WishList tablosu
CREATE TABLE WishList (
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    GameID INT FOREIGN KEY REFERENCES Games(GameID),
    AddedDate DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (UserID, GameID)
)

-- Friends tablosu
CREATE TABLE Friends (
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    FriendID INT FOREIGN KEY REFERENCES Users(UserID),
    FriendshipDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Pending',
    PRIMARY KEY (UserID, FriendID)
)

-- Achievements tablosu
CREATE TABLE Achievements (
    AchievementID INT PRIMARY KEY IDENTITY(1,1),
    GameID INT FOREIGN KEY REFERENCES Games(GameID),
    Title NVARCHAR(100) NOT NULL,
    Description NTEXT,
    IconUrl NVARCHAR(255)
)

-- UserAchievements tablosu
CREATE TABLE UserAchievements (
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    AchievementID INT FOREIGN KEY REFERENCES Achievements(AchievementID),
    UnlockDate DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (UserID, AchievementID)
) 