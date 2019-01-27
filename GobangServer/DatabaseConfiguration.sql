-- Drop all existing tables so that this script can be used to initialize the database configuration on a new computer.

DROP DATABASE IF EXISTS Gobang;

CREATE DATABASE Gobang;
USE Gobang;
Go

DROP TABLE IF EXISTS Users;
CREATE TABLE Users
(
    Account NVARCHAR(20) PRIMARY KEY,
    Password NVARCHAR(32), -- Encrypted 32-bit MD5 code.
    MailAddress NVARCHAR(50)
);

