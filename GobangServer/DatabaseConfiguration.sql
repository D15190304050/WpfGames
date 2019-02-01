-- Drop all existing tables so that this script can be used to initialize the database configuration on a new computer.

-- Use master instead of Gobang so that the Gobang database can be dropped.
USE master;
GO

DECLARE @dbname SYSNAME
SET @dbname = 'Gobang' -- Put the name of the databse to drop here.

DECLARE @s NVARCHAR(1000)
DECLARE tb CURSOR LOCAL
FOR
    SELECT s = 'kill ' + CAST(spid AS VARCHAR)
    FROM MASTER..sysprocesses
    WHERE dbid = DB_ID(@dbname)

OPEN tb
FETCH NEXT FROM tb INTO @s
WHILE @@fetch_status = 0
BEGIN
    EXEC(@s)
    FETCH NEXT FROM tb INTO @s
END
CLOSE tb
DEALLOCATE tb

EXEC('drop database [' + @dbname + ']');

CREATE DATABASE Gobang;
USE Gobang;
GO

DROP TABLE IF EXISTS Users;
CREATE TABLE Users
(
    Account NVARCHAR(20) PRIMARY KEY,
    Password NVARCHAR(32), -- Encrypted 32-bit MD5 code.
    MailAddress NVARCHAR(50)
);

-- Uncomment the following command to show data contained in the "Users" table.
SELECT * FROM Users;

SELECT Password
FROM Users
WHERE Account = 'DeadSpace';
