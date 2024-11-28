USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'BookLibrary')
BEGIN
    CREATE DATABASE [BookLibrary] COLLATE SQL_Latin1_General_CP1_CI_AS;
END
GO

USE master;
GO

IF EXISTS (SELECT 1 FROM master.sys.server_principals WHERE name = 'developer')
BEGIN
	EXEC master..sp_addsrvrolemember @loginame = 'developer', @rolename = 'sysadmin';
END
GO