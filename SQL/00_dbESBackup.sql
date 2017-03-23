/*
	- OK - Klienti = účty
	- OK - Nastavení záloh
	- OK - Historie záloh (zálohy)
	- OK - Logy
	- OK - Maily, ... (actions)
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
CREATE TABLE esbk_tbClientLogins(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int

	LG_TIME datetime not null, -- datetime
	LG_CLIENT_IP varbinary(128) not null, -- Byte[]; IPv4 - 32 bitů, IPv6 - 128 bitů
);

CREATE TABLE esbk_tbBackups(
	ID bigint identity(1,1) not null, -- long
	IDesbk_tbClients int not null, -- int

	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string
	
	BK_TIME_BEGIN datetime not null, -- datetime
	BK_TIME_END datetime, -- datetime?, null - updatem

	BK_EXPIRATION datetime, -- datetime?
	BK_COMPRESSION char(1) -- C = compress, N = do not compress
);
CREATE TABLE esbk_tbBackupDetails(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbBackups bigint not null, -- long
	BK_PATH varchar(4096), -- string

	-- k ověření
	BK_TIME datetime not null, -- datetime, čas provedení zálohy
	BK_LAST_CHANGE datetime not null, -- datetime - získání ze souboru
	BK_HASH varchar(2048), -- string
);

CREATE TABLE esbk_tbBackupSetting(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int
	IDesbk_tbBackupSettingTypes int not null, -- int
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

	AC_VALUE varchar(max) not null, -- parameters (path src=C: dst=D:, email, ...)

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
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbBackups bigint not null, -- long
	LG_TIME datetime not null, -- datetime
	LG_VALUE varchar(max) not null -- string
);

BEGIN /* PK */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT PK_esbk_tbClients_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT PK_esbk_tbClientLogins_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT PK_esbk_tbBackups_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT PK_esbk_tbBackupDetails_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackupSetting ADD CONSTRAINT PK_esbk_tbBackupSetting_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackupSettingTypes ADD CONSTRAINT PK_esbk_tbBackupSettingTypes_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupActions ADD CONSTRAINT PK_esbk_tbBackupActions_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackupActionTypes ADD CONSTRAINT PK_esbk_tbBackupActionTypes_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupLogs ADD CONSTRAINT PK_esbk_tbBackupLogs_ID PRIMARY KEY NONCLUSTERED (ID);
END
BEGIN /* FK */
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT FK_esbk_tbClientLogins_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT FK_esbk_tbBackupDetails_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbBackupSetting ADD CONSTRAINT FK_esbk_tbBackupSetting_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackupSetting ADD CONSTRAINT FK_esbk_tbBackupSetting_IDesbk_tbBackupSettingTypes FOREIGN KEY (IDesbk_tbBackupSettingTypes) REFERENCES esbk_tbBackupSettingTypes(ID);
	ALTER TABLE esbk_tbBackupActions ADD CONSTRAINT FK_esbk_tbBackupActions_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbBackupActions ADD CONSTRAINT FK_esbk_tbBackupActions_IDesbk_tbBackupActionTypes FOREIGN KEY (IDesbk_tbBackupActionTypes) REFERENCES esbk_tbBackupActionTypes(ID);
	ALTER TABLE esbk_tbBackupLogs ADD CONSTRAINT FK_esbk_tbBackupLogs_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID);
END
BEGIN /* IX */
	CREATE INDEX IX_esbk_tbClientLogins_IDesbk_tbClients ON esbk_tbClientLogins(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbClients ON esbk_tbBackups(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackupDetails_IDesbk_tbBackups ON esbk_tbBackupDetails(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbBackupSetting_IDesbk_tbClients ON esbk_tbBackupSetting(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackupSetting_IDesbk_tbBackupSettingTypes ON esbk_tbBackupSetting(IDesbk_tbBackupSettingTypes);
	CREATE INDEX IX_esbk_tbBackupActions_IDesbk_tbBackups ON esbk_tbBackupActions(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbBackupActions_IDesbk_tbBackupActionTypes ON esbk_tbBackupActions(IDesbk_tbBackupActionTypes);
END
BEGIN /* DF */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_VERIFIED DEFAULT (0) FOR CL_VERIFIED;
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT DF_esbk_tbClientLogins_LG_TIME DEFAULT (GETDATE()) FOR LG_TIME;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TIME_BEGIN DEFAULT (GETDATE()) FOR BK_TIME_BEGIN;
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT DF_esbk_tbBackupDetails_BK_TIME DEFAULT (GETDATE()) FOR BK_TIME;
	ALTER TABLE esbk_tbBackupLogs ADD CONSTRAINT DF_esbk_tbBackupLogs_LG_TIME DEFAULT (GETDATE()) FOR LG_TIME;
END
BEGIN /* CK */
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT CK_esbk_tbClientLogins_LG_TIME CHECK (LG_TIME <= GETDATE());
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT CK_esbk_tbClientLogins_LG_CLIENT_IP CHECK (LEN(LG_CLIENT_IP) = 32 OR LEN(LG_CLIENT_IP) = 128); -- IPv4 || IPv6
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_BEGIN CHECK (BK_TIME_BEGIN <= GETDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_END CHECK (BK_TIME_BEGIN <= BK_TIME_END AND BK_TIME_END <= GETDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_EXPIRATION CHECK (BK_EXPIRATION >= GETDATE());
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT CK_esbk_tbBackupDetails_BK_TIME CHECK (BK_TIME <= GETDATE());
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT CK_esbk_tbBackupDetails_BK_LAST_CHANGE CHECK (BK_LAST_CHANGE <= GETDATE());
	ALTER TABLE esbk_tbBackupLogs ADD CONSTRAINT CK_esbk_tbBackupLogs_LG_TIME CHECK (LG_TIME <= GETDATE());
END