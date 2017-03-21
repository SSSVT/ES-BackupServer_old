/*
	- Klienti = úèty
	- Nastavení záloh
	- Historie záloh
	- Logy
	- Maily
	- Historie pøihlášení
*/

USE [master];

IF EXISTS(SELECT * FROM sys.databases WHERE name='dbESBackup')
BEGIN
	ALTER DATABASE [dbESBackup] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [dbESBackup];
END

CREATE DATABASE [dbESBackup];
GO

USE [dbESBackup];

CREATE TABLE esbk_tbClients(
	ID int identity(1,1) not null, -- int
	CL_NAME varchar(64) not null, -- string
	CL_DESCRIPTION varchar(512), -- string

	-- return true/false = login failed
	CL_LOGIN_NAME varchar(128) not null, -- username
	CL_LOGIN_PSWD varchar(2048) not null, -- password
	CL_LOGIN_SALT varchar(512) not null, -- salt

	CL_VERIFIED bit not null -- bool
);
CREATE TABLE esbk_tbBackups(
	ID bigint identity(1,1) not null, -- long
	BK_IDesbk_tbClients int not null, -- int

	-- k ovìøení
	BK_TIME_BEGIN datetime not null, -- datetime
	BK_TIME_END datetime, -- datetime?, null - updatem
);

CREATE TABLE esbk_tbBackupSetting(
	ID uniqueidentifier not null, -- GUID
	BK_IDesbk_tbClients int not null, -- int
	BK_IDesbk_tbBackupSettingTypes int not null, -- int
	BK_VALUE varchar(4096) not null -- string
);
CREATE TABLE esbk_tbBackupSettingTypes(
	ID int identity(1,1) not null,
	TP_NAME varchar(64) not null
);

CREATE TABLE esbk_tbBackupDetails(
	ID uniqueidentifier not null, -- GUID
	BK_PATH varchar(4096), -- string

	-- k ovìøení
	BK_TIME datetime not null, -- datetime
	BK_LAST_CHANGE datetime not null, -- datetime
	BK_HASK varchar(2048), -- string
);
CREATE TABLE esbk_tbClientLogins(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int

	LG_TIME datetime not null, -- datetime
	LG_CLIENT_IP varbinary(128) not null, -- Byte[]; IPv4 - 32 bitù, IPv6 - 128 bitù
);