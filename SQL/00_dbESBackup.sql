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

	-- info o backupech (kvůli selectům a nastavení) - 
	CL_LAST_BACKUP datetime, -- nemusel ještě proběhnout žádný
	--CL_BACKUP_AUTOREPEAT_TIME bigint, -- časová prodleva mezi backupy (v minutách)
	--CL_BACKUP_ATTEMPTS int not null,
	
	CL_VERIFIED bit not null -- bool, autorizován
); /* Tabulka serverů (klientů), kteří se zálohují */
CREATE TABLE esbk_tbClientLogins(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int

	LG_TIME_UTC datetime not null, -- datetime
	LG_CLIENT_IP varbinary(128) not null, -- byte[]; IPv4 - 32 bitů, IPv6 - 128 bitů

	LG_ACTIVE bit not null
); /* Historie přihlášení klientů */

CREATE TABLE esbk_tbClientSetting(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int
	IDesbk_tbClientSettingTypes int not null, -- int

	-- DISABLED/TIME/BEFORE/AFTER BACKUP
	-- 0 = event (before/after);
	-- 1 = time
	-- null != event || time
	ST_ACTION_TYPE bit, -- bool?
	ST_EVENT bit, -- 0 = before backup, 1 = after backup
	ST_TIME datetime, -- datetime?

	ST_VALUE varchar(max) -- string
);
CREATE TABLE esbk_tbClientSettingTypes(
	ID int identity(1,1) not null,
	TP_NAME varchar(64) not null
);

CREATE TABLE esbk_tbBackups(
	ID bigint identity(1,1) not null, -- long
	IDesbk_tbClients int not null, -- int

	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string
	
	BK_TIME_BEGIN datetime not null, -- datetime
	BK_TIME_END datetime, -- datetime?, null - updatem

	BK_EXPIRATION datetime, -- datetime?
	BK_COMPRESSION bit not null -- 0 = do not compress; 1 = compress
); /* Historie provedených záloh */
CREATE TABLE esbk_tbBackupDetails(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbBackups bigint not null, -- long
	BK_PATH_SOURCE varchar(max), -- string
	BK_PATH_DESTINATION varchar(max), -- string

	-- k ověření
	BK_TIME datetime not null, -- datetime, čas provedení zálohy
	BK_LAST_CHANGE datetime not null, -- datetime - získání ze souboru
	BK_HASH varchar(4096), -- string
); /* Info o jednotlivých zálohách (tbRozpisObjednávek) */

CREATE TABLE esbk_tbClientLogs(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- musí se vázat ke klientovi
	IDesbk_tbBackups bigint, -- long? - nemusí se vázat k záloze
	IDesbk_tbClientLogTypes tinyint not null, -- byte
	LG_TIME_UTC datetime not null, -- datetime
	LG_VALUE varchar(max) not null -- string
); /* Logy */
CREATE TABLE esbk_tbClientLogTypes(
	ID tinyint identity(1,1) not null, -- byte 0-255
	TP_NAME varchar(64) not null
); /* Typy logů */

BEGIN /* PK */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT PK_esbk_tbClients_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT PK_esbk_tbClientLogins_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT PK_esbk_tbBackups_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT PK_esbk_tbBackupDetails_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbClientSetting ADD CONSTRAINT PK_esbk_tbClientSetting_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbClientSettingTypes ADD CONSTRAINT PK_esbk_tbClientSettingTypes_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT PK_esbk_tbClientLogs_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbClientLogTypes ADD CONSTRAINT PK_esbk_tbClientLogTypes_ID PRIMARY KEY (ID);
END
BEGIN /* FK */
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT FK_esbk_tbClientLogins_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT FK_esbk_tbBackupDetails_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbClientSetting ADD CONSTRAINT FK_esbk_tbClientSetting_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbClientSetting ADD CONSTRAINT FK_esbk_tbClientSetting_IDesbk_tbClientSettingTypes FOREIGN KEY (IDesbk_tbClientSettingTypes) REFERENCES esbk_tbClientSettingTypes(ID);
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT FK_esbk_tbClientLogs_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT FK_esbk_tbClientLogs_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT FK_esbk_tbClientLogs_IDesbk_tbClientLogTypes FOREIGN KEY (IDesbk_tbClientLogTypes) REFERENCES esbk_tbClientLogTypes(ID);
END
BEGIN /* IX */
	CREATE INDEX IX_esbk_tbClientLogins_ID ON esbk_tbClientLogins(ID);
	CREATE INDEX IX_esbk_tbClientLogins_IDesbk_tbClients_LG_TIME ON esbk_tbClientLogins(IDesbk_tbClients, LG_TIME);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbClients ON esbk_tbBackups(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackupDetails_IDesbk_tbBackups ON esbk_tbBackupDetails(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbClientSetting_IDesbk_tbClients ON esbk_tbClientSetting(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbClientSetting_IDesbk_tbClientSettingTypes ON esbk_tbClientSetting(IDesbk_tbClientSettingTypes);
	CREATE INDEX IX_esbk_tbClientLogs_IDesbk_tbClients ON esbk_tbClientLogs(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbClientLogs_IDesbk_tbBackups ON esbk_tbClientLogs(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbClientLogs_IDesbk_tbClientLogTypes ON esbk_tbClientLogs(IDesbk_tbClientLogTypes);
END
BEGIN /* DF */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_VERIFIED DEFAULT (0) FOR CL_VERIFIED;
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT DF_esbk_tbClientLogins_LG_TIME_UTC DEFAULT (GETDATE()) FOR LG_TIME_UTC;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TIME_BEGIN DEFAULT (GETDATE()) FOR BK_TIME_BEGIN;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_COMPRESSION DEFAULT (0) FOR BK_COMPRESSION;
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT DF_esbk_tbBackupDetails_BK_TIME DEFAULT (GETDATE()) FOR BK_TIME;
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT DF_esbk_tbClientLogs_LG_TIME_UTC DEFAULT (GETDATE()) FOR LG_TIME_UTC;
END
BEGIN /* CK */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT CK_esbk_tbClients_CL_LAST_BACKUP CHECK (CL_LAST_BACKUP <= GETDATE());
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT CK_esbk_tbClientLogins_LG_TIME CHECK (LG_TIME <= GETDATE());
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT CK_esbk_tbClientLogins_LG_CLIENT_IP CHECK (LEN(LG_CLIENT_IP) = 32 OR LEN(LG_CLIENT_IP) = 128); -- IPv4 || IPv6
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_BEGIN CHECK (BK_TIME_BEGIN <= GETDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_END CHECK (BK_TIME_BEGIN <= BK_TIME_END AND BK_TIME_END <= GETDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_EXPIRATION CHECK (BK_EXPIRATION >= GETDATE());
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT CK_esbk_tbBackupDetails_BK_TIME CHECK (BK_TIME <= GETDATE());
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT CK_esbk_tbBackupDetails_BK_LAST_CHANGE CHECK (BK_LAST_CHANGE <= GETDATE());
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT CK_esbk_tbClientLogs_LG_TIME CHECK (LG_TIME <= GETDATE());
END
BEGIN /* UQ */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT UQ_esbk_tbClients_CL_LOGIN_NAME UNIQUE (CL_LOGIN_NAME);
	ALTER TABLE esbk_tbClientSettingTypes ADD CONSTRAINT UQ_esbk_tbClientSettingTypes_TP_NAME UNIQUE (TP_NAME);
	ALTER TABLE esbk_tbClientLogTypes ADD CONSTRAINT UQ_esbk_tbClientLogTypes_TP_NAME UNIQUE (TP_NAME);
END

BEGIN /* INSERT INTO esbk_tbClientSettingTypes */
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Ignore'); -- ignorovat cestu, soubory, ...
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Only'); -- pouze cestu, soubory, ...
	INSERT INTO esbk_tbClientSettingTypes VALUES ('PathSource'); -- cesta zdroj
	INSERT INTO esbk_tbClientSettingTypes VALUES ('PathDestination'); -- cesta cíl
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Compression'); -- komprese zálohy (.zip)

	INSERT INTO esbk_tbClientSettingTypes VALUES ('Start'); -- Start backup
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Resume'); -- Resume backup
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Pause'); -- Pause backup
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Stop'); -- Stop backup

	INSERT INTO esbk_tbClientSettingTypes VALUES ('ShutDown');
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Restart');
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Sleep');
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Hibernate');
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Lock');

	INSERT INTO esbk_tbClientSettingTypes VALUES ('Email');
	INSERT INTO esbk_tbClientSettingTypes VALUES ('Notification'); -- notifikace na obrazovce
END
BEGIN /* INSERT INTO esbk_tbClientLogTypes */
	INSERT INTO esbk_tbClientLogTypes VALUES ('Error'); -- exceptions
	INSERT INTO esbk_tbClientLogTypes VALUES ('Warning'); -- Špatné přihlašovací údaje, ...
	INSERT INTO esbk_tbClientLogTypes VALUES ('Message'); -- finished, email sent, ...
END