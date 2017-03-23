/*
	- OK - Klienti = účty
	- OK - Nastavení záloh
	- OK - Historie záloh (zálohy)
	- OK - Logy
	- Maily
	- OK - Historie přihlášení
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
	CL_HWID varchar(512), -- HWID

	-- return true/false = login failed
	CL_LOGIN_NAME varchar(128) not null, -- username
	CL_LOGIN_PSWD varchar(2048) not null, -- password
	CL_LOGIN_SALT varchar(512) not null, -- salt

	CL_VERIFIED bit not null -- bool
);
CREATE TABLE esbk_tbBackups(
	ID bigint identity(1,1) not null, -- long
	BK_IDesbk_tbClients int not null, -- int

	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string
	
	BK_TIME_BEGIN datetime not null, -- datetime
	BK_TIME_END datetime, -- datetime?, null - updatem

	BK_EXPIRATION datetime, -- datetime?
	BK_COMPRESSION char(1) -- C = compress, N = do not compress
);

CREATE TABLE esbk_tbBackupSetting(
	ID uniqueidentifier not null, -- GUID
	BK_IDesbk_tbClients int not null, -- int
	BK_IDesbk_tbBackupSettingTypes int not null, -- int
	BK_VALUE varchar(4096) not null -- string
);
CREATE TABLE esbk_tbBackupSettingTypes(
	ID int identity(1,1) not null,
	TP_NAME varchar(64) not null -- ingore, path, ...
);

CREATE TABLE esbk_tbBackupActions(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbBackups bigint not null,
	IDesbk_tbBackupActionTypes int not null,

	AC_VALUE varchar(max) not null, -- parameters (path, email, ...)

	-- TIME/BEFORE/AFTER BACKUP
	AC_ACTION_TYPE bit not null, -- 0 = event (before/after); 1 = time
	AC_EVENT int, -- int
	AC_TIME datetime, -- datetime?
);
CREATE TABLE esbk_tbBackupActionTypes(
	ID int identity(1,1) not null, -- int
	TP_NAME varchar(64) not null
);
CREATE TABLE esbk_tbBackupLogs(
	ID uniqueidentifier not null,
	IDesbk_tbBackups bigint not null,
	LG_TIME datetime not null,
	LG_VALUE varchar(max) not null
);

CREATE TABLE esbk_tbBackupDetails(
	ID uniqueidentifier not null, -- GUID
	BK_PATH varchar(4096), -- string

	-- k ověření
	BK_TIME datetime not null, -- datetime
	BK_LAST_CHANGE datetime not null, -- datetime
	BK_HASH varchar(2048), -- string
);
CREATE TABLE esbk_tbClientLogins(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int

	LG_TIME datetime not null, -- datetime
	LG_CLIENT_IP varbinary(128) not null, -- Byte[]; IPv4 - 32 bitů, IPv6 - 128 bitů
);

/* PK */
ALTER TABLE esbk_tbClients ADD CONSTRAINT PK_esbk_tbClients_ID PRIMARY KEY (ID);
ALTER TABLE esbk_tbBackups ADD CONSTRAINT PK_esbk_tbBackups_ID PRIMARY KEY (ID);
ALTER TABLE esbk_tbBackupSetting ADD CONSTRAINT PK_esbk_tbBackupSetting_ID PRIMARY KEY NONCLUSTERED (ID);
ALTER TABLE esbk_tbBackupSettingTypes ADD CONSTRAINT PK_esbk_tbBackupSettingTypes_ID PRIMARY KEY (ID);
ALTER TABLE esbk_tbBackupActions ADD CONSTRAINT PK_esbk_tbBackupActions_ID PRIMARY KEY NONCLUSTERED (ID);
