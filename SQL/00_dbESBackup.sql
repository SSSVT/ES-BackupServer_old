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
	CL_EMAILS varchar(512), --string, seperated by ';'

	-- return true/false = login failed
	CL_LOGIN_NAME varchar(128), -- username
	CL_LOGIN_PSWD varchar(2048), -- password
	CL_LOGIN_SALT varchar(512), -- salt

	CL_LAST_BACKUP datetime, -- datetime?
	
	CL_STATUS tinyint not null, -- byte, autorizován/request registration/banned

	CL_AUTO_STATUS_REPORT_ENABLED bit not null,
	CL_AUTO_STATUS_REPORT_INTERVAL int,
	CL_LAST_STATUS_REPORT datetime,

); /* Tabulka serverů (klientů), kteří se zálohují */
CREATE TABLE esbk_tbLogins(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int

	LG_TIME_UTC datetime not null, -- datetime
	LG_TIME_EXPIRATION_UTC datetime not null, -- datetime; default - 15 minut; při dalším requestu obnovit na 15
	LG_CLIENT_IP varbinary(128) not null, -- byte[]; IPv4 - 32 bitů, IPv6 - 128 bitů
); /* Historie přihlášení klientů */

CREATE TABLE esbk_tbBackups(
	ID bigint identity(1,1) not null, -- long
	IDesbk_tbClients int not null, -- int
	IDesbk_tbBackupTemplates bigint not null,
	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string
	
	BK_SOURCE varchar(max) not null, -- new
	BK_DESTINATION varchar(max) not null, -- new

	BK_TYPE bit not null, -- 0 = full, 1 = differential
	IDesbk_tbBackups_BASE bigint,

	BK_EXPIRATION datetime, -- datetime?; new here
	BK_COMPRESSION bit not null, -- 0 = do not compress; 1 = compress; new here

	BK_TIME_BEGIN datetime not null, -- datetime
	BK_TIME_END datetime, -- datetime?, null - updatem
	BK_STATUS tinyint not null -- Executing, Competed, Failed, ...
); /* Historie provedených záloh */
CREATE TABLE esbk_tbBackupTemplates(
	ID bigint identity(1,1) not null,
	IDesbk_tbClients int not null,
	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string

	BK_TYPE bit not null, -- 0 = full, 1 = differential

	BK_EXPIRATION_DAYS int, -- int?
	BK_COMPRESSION bit not null, -- 0 = do not compress; 1 = compress

	BK_ENABLED bit not null -- zda je template aktivní
);

CREATE TABLE esbk_tbBackupTemplatesPathInfo(
	ID int identity(1,1) not null,
	IDesbk_tbBackupTemplates bigint not null,
	PI_SOURCE varchar(max) not null, -- zdroj
	PI_DESTINATION varchar(max) not null, -- cíl
	PI_RESTRICTION bit null, -- ONLY = 0, IGNORE = 1, NULL = ALL
	PI_RESTRICTION_VALUE varchar(512) null, -- Povolení/Odepření
	PI_TYPE bit not null -- 0 = Win to Win / 1 = Win to FTP
);

CREATE TABLE esbk_tbBackupTemplatesTimeSetting(
	ID int identity(1,1) not null,
	IDesbk_tbBackupTemplates bigint not null,
	TS_TIME datetime not null, -- Time when executed
	TS_REPEAT bit not null, -- 0 = No Repeat / 1 = Repeat 
	TS_REPEAT_CRON varchar(512) null -- CRON Value
);

CREATE TABLE esbk_tbBackupTemplatesCommands(
	ID int identity(1,1) not null,
	IDesbk_tbBackupTemplates bigint not null,
	CM_EVENT bit not null,--0 = before backup, 1 = after backup
	CM_TYPE bit not null, --0 = Email, 1 = Notification
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
	ALTER TABLE esbk_tbBackupTemplatesPathInfo ADD CONSTRAINT PK_esbk_tbBackupTemplatesPathInfo_ID PRIMARY KEY(ID);
	ALTER TABLE esbk_tbBackupTemplatesTimeSetting ADD CONSTRAINT PK_esbk_tbBackupTemplatesTimeSetting_ID PRIMARY KEY(ID);
	ALTER TABLE esbk_tbBackupTemplatesCommands ADD CONSTRAINT PK_esbk_tbBackupTemplatesCommands_ID PRIMARY KEY(ID);
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT PK_esbk_tbLogs_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbLogTypes ADD CONSTRAINT PK_esbk_tbLogTypes_ID PRIMARY KEY (ID);
END
BEGIN /* FK */
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT FK_esbk_tbLogins_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_IDesbk_tbBackups_BASE FOREIGN KEY (IDesbk_tbBackups_BASE) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT FK_esbk_tbBackupTemplates_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackupTemplatesPathInfo ADD CONSTRAINT FK_esbk_tbBackupTemplatesPathInfo_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID);
	ALTER TABLE esbk_tbBackupTemplatesTimeSetting ADD CONSTRAINT FK_esbk_tbBackupTemplatesTimeSetting_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID);
	ALTER TABLE esbk_tbBackupTemplatesCommands ADD CONSTRAINT FK_esbk_tbBackupTemplatesCommands_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID);	
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID); -- zůstanou logy, které nepatří k backupu - klienti se tak často odstraňovat nebudou
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbLogTypes FOREIGN KEY (IDesbk_tbLogTypes) REFERENCES esbk_tbLogTypes(ID); -- Typy logů odstraňovat nebudeme, dostačující
END
BEGIN /* IX */
	CREATE UNIQUE INDEX IX_UQ_esbk_tbClients ON esbk_tbClients(CL_LOGIN_NAME);
	CREATE INDEX IX_esbk_tbLogins_ID ON esbk_tbLogins(ID);
	CREATE INDEX IX_esbk_tbLogins_IDesbk_tbClients_LG_TIME_UTC ON esbk_tbLogins(IDesbk_tbClients, LG_TIME_UTC);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbClients ON esbk_tbBackups(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbBackups_BASE ON esbk_tbBackups(IDesbk_tbBackups_BASE) WHERE BK_TYPE = 1;

	-- TODO

	CREATE INDEX IX_esbk_tbBackupTemplates_IDesbk_tbClients ON esbk_tbBackupTemplates(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbClients ON esbk_tbLogs(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbBackups ON esbk_tbLogs(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbLogTypes ON esbk_tbLogs(IDesbk_tbLogTypes);
END
BEGIN /* DF */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_STATUS DEFAULT (0) FOR CL_STATUS;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT DF_esbk_tbLogins_LG_TIME_UTC DEFAULT (GETDATE()) FOR LG_TIME_UTC;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT DF_esbk_tbLogins_LG_TIME_EXPIRATION_UTC DEFAULT (DATEADD(minute, 15, GETDATE())) FOR LG_TIME_EXPIRATION_UTC;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TYPE DEFAULT (0) FOR BK_TYPE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_COMPRESSION DEFAULT (0) FOR BK_COMPRESSION;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TIME_BEGIN DEFAULT (GETDATE()) FOR BK_TIME_BEGIN;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_STATUS DEFAULT (0) FOR BK_STATUS;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_TYPE DEFAULT (0) FOR BK_TYPE;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_COMPRESSION DEFAULT (0) FOR BK_COMPRESSION;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_ENABLED DEFAULT (0) FOR BK_ENABLED;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT DF_esbk_tbLogs_LG_TIME_UTC DEFAULT (GETDATE()) FOR LG_TIME_UTC;
END
BEGIN /* CK */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT CK_esbk_tbClients_CL_LAST_BACKUP CHECK (CL_LAST_BACKUP <= GETDATE());
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT CK_esbk_tbLogins_LG_TIME_UTC CHECK (LG_TIME_UTC <= GETDATE());
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT CK_esbk_tbLogins_LG_TIME_EXPIRATION_UTC CHECK (LG_TIME_UTC <= LG_TIME_EXPIRATION_UTC);
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT CK_esbk_tbLogins_LG_CLIENT_IP CHECK (LEN(LG_CLIENT_IP) = 32 OR LEN(LG_CLIENT_IP) = 128); -- IPv4 || IPv6
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_EXPIRATION CHECK (BK_EXPIRATION >= GETDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_BEGIN CHECK (BK_TIME_BEGIN <= GETDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_END CHECK (BK_TIME_BEGIN <= BK_TIME_END AND BK_TIME_END <= GETDATE());
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT CK_esbk_tbBackupTemplates_BK_EXPIRATION_DAYS CHECK (BK_EXPIRATION_DAYS > 0);
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT CK_esbk_tbLogs_LG_TIME_UTC CHECK (LG_TIME_UTC <= GETDATE());
END
BEGIN /* UQ */	
	ALTER TABLE esbk_tbLogTypes ADD CONSTRAINT UQ_esbk_tbLogTypes_TP_NAME UNIQUE (TP_NAME);
END
BEGIN /* INSERT INTO esbk_tbLogTypes */
	INSERT INTO esbk_tbLogTypes VALUES ('Error'); -- exceptions
	INSERT INTO esbk_tbLogTypes VALUES ('Warning'); -- Špatné přihlašovací údaje, ...
	INSERT INTO esbk_tbLogTypes VALUES ('Message'); -- finished, email sent, ...
END