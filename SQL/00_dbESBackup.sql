USE [master];
IF EXISTS(SELECT * FROM sys.databases WHERE name='dbESBackup')
BEGIN
	ALTER DATABASE [dbESBackup] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [dbESBackup];
END

CREATE DATABASE [dbESBackup];
GO

/* EDIT CONSTRAINTS */

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

	CL_LAST_BACKUP datetime, -- datetime?
	
	CL_VERIFIED bit not null -- bool, autorizován
); /* Tabulka serverů (klientů), kteří se zálohují */
CREATE TABLE esbk_tbLogins(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int

	LG_TIME_UTC datetime not null, -- datetime
	LG_TIME_EXPIRATION datetime not null, -- datetime; default - 15 minut; při dalším requestu obnovit na 15
	LG_CLIENT_IP varbinary(128) not null, -- byte[]; IPv4 - 32 bitů, IPv6 - 128 bitů
); /* Historie přihlášení klientů */

CREATE TABLE esbk_tbBackups(
	ID bigint identity(1,1) not null, -- long
	IDesbk_tbClients int not null, -- int
	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string
	
	BK_SOURCE varchar(max) not null, -- new
	BK_DESTINATION varchar(max) not null, -- new

	BK_TYPE bit not null, -- 0 = full, 1 = differential

	BK_EXPIRATION datetime, -- datetime?; new here
	BK_COMPRESSION bit not null, -- 0 = do not compress; 1 = compress; new here

	BK_TIME_BEGIN datetime not null, -- datetime
	BK_TIME_END datetime, -- datetime?, null - updatem
	BK_STATUS tinyint not null -- Competed, Failed, ...
); /* Historie provedených záloh */
CREATE TABLE esbk_tbBackupTemplates(
	ID bigint identity(1,1) not null,
	IDesbk_tbClients int not null,
	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string

	BK_SOURCE varchar(max) not null, -- new
	BK_DESTINATION varchar(max) not null, -- new

	BK_TYPE bit not null, -- 0 = full, 1 = differential

	BK_EXPIRATION_DAYS int, -- int?
	BK_COMPRESSION bit not null, -- 0 = do not compress; 1 = compress
);
CREATE TABLE esbk_tbBackupTemplatesSetting(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbBackupTemplates bigint not null, -- int
	IDesbk_tbBackupTemplatesSettingTypes int not null, -- int

	-- DISABLED/TIME/BEFORE/AFTER BACKUP
	-- null != event || time
	-- 0 = event (before/after);
	-- 1 = time	
	ST_ACTION_TYPE bit, -- bool?
	ST_EVENT bit, -- 0 = before backup, 1 = after backup
	ST_TIME datetime, -- datetime?

	ST_VALUE varchar(max) -- string
);
CREATE TABLE esbk_tbBackupTemplatesSettingTypes(
	ID int identity(1,1) not null,
	TP_NAME varchar(64) not null
);

CREATE TABLE esbk_tbLogs(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- musí se vázat ke klientovi
	IDesbk_tbBackups bigint, -- long? - nemusí se vázat k záloze
	IDesbk_tbLogTypes tinyint not null, -- byte
	LG_TIME_UTC datetime not null, -- datetime
	LG_VALUE varchar(max) not null -- string
); /* Logy */
CREATE TABLE esbk_tbLogTypes(
	ID tinyint identity(1,1) not null, -- byte 0-255
	TP_NAME varchar(64) not null
); /* Typy logů */

BEGIN /* PK */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT PK_esbk_tbClients_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT PK_esbk_tbLogins_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT PK_esbk_tbBackups_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT PK_esbk_tbBackupTemplates_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupTemplatesSetting ADD CONSTRAINT PK_esbk_tbBackupTemplatesSetting_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackupTemplatesSettingTypes ADD CONSTRAINT PK_esbk_tbBackupTemplatesSettingTypes_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT PK_esbk_tbLogs_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbLogTypes ADD CONSTRAINT PK_esbk_tbLogTypes_ID PRIMARY KEY (ID);
END
BEGIN /* FK */
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT FK_esbk_tbLogins_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT FK_esbk_tbBackupTemplates_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackupTemplatesSetting ADD CONSTRAINT FK_esbk_tbBackupTemplatesSetting_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackupTemplatesSetting ADD CONSTRAINT FK_esbk_tbBackupTemplatesSetting_IDesbk_tbBackupTemplatesSettingTypes FOREIGN KEY (IDesbk_tbBackupTemplatesSettingTypes) REFERENCES esbk_tbBackupTemplatesSettingTypes(ID); -- nechceme zrušit uživatelům nastavení
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID); -- zůstanou logy, které nepatří k backupu - klienti se tak často odstraňovat nebudou
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbLogTypes FOREIGN KEY (IDesbk_tbLogTypes) REFERENCES esbk_tbLogTypes(ID); -- Typy logů odstraňovat nebudeme, dostačující
END
BEGIN /* IX */
	CREATE UNIQUE INDEX IX_UQ_esbk_tbClients ON esbk_tbClients(CL_LOGIN_NAME);
	CREATE INDEX IX_esbk_tbLogins_ID ON esbk_tbLogins(ID);
	CREATE INDEX IX_esbk_tbLogins_IDesbk_tbClients_LG_TIME_UTC ON esbk_tbLogins(IDesbk_tbClients, LG_TIME_UTC);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbClients ON esbk_tbBackups(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackupTemplates_IDesbk_tbClients ON esbk_tbBackupTemplates(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackupTemplatesSetting_IDesbk_tbBackupTemplates ON esbk_tbBackupTemplatesSetting(IDesbk_tbBackupTemplates);
	CREATE INDEX IX_esbk_tbBackupTemplatesSetting_IDesbk_tbBackupTemplatesSettingTypes ON esbk_tbBackupTemplatesSetting(IDesbk_tbBackupTemplatesSettingTypes);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbClients ON esbk_tbLogs(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbBackups ON esbk_tbLogs(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbLogTypes ON esbk_tbLogs(IDesbk_tbLogTypes);
END
BEGIN /* DF */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_VERIFIED DEFAULT (0) FOR CL_VERIFIED;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT DF_esbk_tbLogins_LG_TIME_UTC DEFAULT (GETDATE()) FOR LG_TIME_UTC;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT DF_esbk_tbLogins_LG_TIME_EXPIRATION DEFAULT (DATEADD(minute, 15, GETDATE())) FOR LG_TIME_EXPIRATION;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TYPE DEFAULT (0) FOR BK_TYPE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_COMPRESSION DEFAULT (0) FOR BK_COMPRESSION;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TIME_BEGIN DEFAULT (GETDATE()) FOR BK_TIME_BEGIN;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_STATUS DEFAULT (0) FOR BK_STATUS;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_TYPE DEFAULT (0) FOR BK_TYPE;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_COMPRESSION DEFAULT (0) FOR BK_COMPRESSION;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT DF_esbk_tbLogs_LG_TIME_UTC DEFAULT (GETDATE()) FOR LG_TIME_UTC;
END
BEGIN /* CK */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT CK_esbk_tbClients_CL_LAST_BACKUP CHECK (CL_LAST_BACKUP <= GETDATE());
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT CK_esbk_tbLogins_LG_TIME_UTC CHECK (LG_TIME_UTC <= GETDATE());
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT CK_esbk_tbLogins_LG_TIME_EXPIRATION CHECK (LG_TIME_UTC <= LG_TIME_EXPIRATION);
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT CK_esbk_tbLogins_LG_CLIENT_IP CHECK (LEN(LG_CLIENT_IP) = 32 OR LEN(LG_CLIENT_IP) = 128); -- IPv4 || IPv6
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_EXPIRATION CHECK (BK_EXPIRATION >= GETDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_BEGIN CHECK (BK_TIME_BEGIN <= GETDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_END CHECK (BK_TIME_BEGIN <= BK_TIME_END AND BK_TIME_END <= GETDATE());
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT CK_esbk_tbBackupTemplates_BK_EXPIRATION_DAYS CHECK (BK_EXPIRATION_DAYS > 0);
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT CK_esbk_tbLogs_LG_TIME_UTC CHECK (LG_TIME_UTC <= GETDATE());
END
BEGIN /* UQ */
	ALTER TABLE esbk_tbBackupTemplatesSettingTypes ADD CONSTRAINT UQ_esbk_tbBackupTemplatesSettingTypes_TP_NAME UNIQUE (TP_NAME);
	ALTER TABLE esbk_tbLogTypes ADD CONSTRAINT UQ_esbk_tbLogTypes_TP_NAME UNIQUE (TP_NAME);
END

BEGIN /* INSERT INTO esbk_tbBackupTemplatesSettingTypes */
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Ignore'); -- ignorovat cestu, soubory, ...
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Only'); -- pouze cestu, soubory, ...

	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Start'); -- Start backup
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Resume'); -- Resume backup
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Pause'); -- Pause backup
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Stop'); -- Stop backup

	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('ShutDown');
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Restart');
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Sleep');
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Hibernate');
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Lock');

	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Email'); -- send email
	INSERT INTO esbk_tbBackupTemplatesSettingTypes VALUES ('Notification'); -- notifikace na obrazovce
END
BEGIN /* INSERT INTO esbk_tbLogTypes */
	INSERT INTO esbk_tbLogTypes VALUES ('Error'); -- exceptions
	INSERT INTO esbk_tbLogTypes VALUES ('Warning'); -- Špatné přihlašovací údaje, ...
	INSERT INTO esbk_tbLogTypes VALUES ('Message'); -- finished, email sent, ...
END