USE [master];
IF EXISTS(SELECT * FROM sys.databases WHERE name='dbESBackup')
BEGIN
	ALTER DATABASE [dbESBackup] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [dbESBackup];
END

CREATE DATABASE [dbESBackup];
GO

USE [dbESBackup];

CREATE TABLE esbk_tbAdministrators(
	ID bigint identity(1,1) not null,
	AD_FIRST_NAME nvarchar(50) not null,
	AD_LAST_NAME nvarchar(50) not null,
	AD_META_REGISTRATION_DATE_UTC datetime not null
);
CREATE TABLE esbk_tbEmails(
	ID uniqueidentifier not null,
	IDesbk_tbAdministrators bigint not null,
	EMAIL varchar(512) not null,
	ISDEFAULT bit not null
);

CREATE TABLE esbk_tbClients(
	ID int identity(1,1) not null, -- int
	IDesbk_tbAdministrators bigint not null, -- long
	CL_NAME varchar(64) not null, -- string
	CL_DESCRIPTION varchar(512), -- string
	CL_HWID varchar(512), -- HWID

	CL_LOGIN_NAME varchar(128), -- username
	CL_LOGIN_PSWD varchar(2048), -- password
	-- CL_LOGIN_SALT varchar(512), -- salt
	
	CL_STATUS tinyint not null, -- byte, autorizován/request registration/banned

	CL_AUTO_STATUS_REPORT_ENABLED bit not null,
	CL_AUTO_STATUS_REPORT_INTERVAL int,
	CL_META_LAST_STATUS_REPORT_UTC datetime,

	CL_META_LAST_BACKUP_UTC datetime, -- datetime?
	CL_META_REGISTRATION_DATE_UTC datetime not null,
	
); /* Tabulka serverů (klientů), kteří se zálohují */
CREATE TABLE esbk_tbLogins(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int

	LG_TIME_UTC datetime not null, -- datetime
	LG_TIME_EXPIRATION_UTC datetime not null, -- datetime; default - 15 minut; při dalším requestu obnovit na 15
	LG_CLIENT_IP varchar(64) not null,
); /* Historie přihlášení klientů */

CREATE TABLE esbk_tbBackups(
	ID bigint identity(1,1) not null, -- long
	IDesbk_tbClients int not null, -- int
	IDesbk_tbBackupTemplates bigint not null,

	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string

	BK_TYPE tinyint not null, -- 0 = full, 1 = differential, 2 = incremental
	IDesbk_tbBackups_BASE bigint,

	BK_SOURCE varchar(max) not null, -- new
	BK_DESTINATION varchar(max) not null, -- new
	
	BK_EXPIRATION_UTC datetime, -- datetime?
	BK_COMPRESSION bit not null, -- 0 = do not compress; 1 = compress

	BK_TIME_BEGIN_UTC datetime not null, -- datetime
	BK_TIME_END_UTC datetime, -- datetime?, null - updatem
	BK_STATUS tinyint not null, -- Executing, Competed, Failed, ...

	BK_META_PATH_ORDER smallint not null, -- pořadí cesty
	BK_META_EMAIL_SENT bit not null
); /* Historie provedených záloh */
CREATE TABLE esbk_tbBackupTemplates(
	ID bigint identity(1,1) not null,
	IDesbk_tbClients int not null,

	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string

	BK_TYPE tinyint not null, -- 0 = full, 1 = differential, 2 = incremental

	BK_EXPIRATION_DAYS int, -- int?
	BK_COMPRESSION bit not null, -- 0 = do not compress; 1 = compress
	BK_SEARCH_PATTERN varchar(2048) not null,
	BK_COPY_EMPTY_DIRS bit not null,

	BK_ENABLED bit not null, -- zda je template aktivní

	BK_NOTIFICATION_ENABLED bit not null, -- notifikace na klientovi
	BK_NOTIFICATION_EMAIL_ENABLED bit not null, -- nofitikace emailem
	BK_REPEAT_INTERVAL_CRON varchar(256) not null, -- interval opakování
);
CREATE TABLE esbk_tbBackupTemplatesPaths(
	ID uniqueidentifier not null,
	IDesbk_tbBackupTemplates bigint not null,

	BK_PATH_ORDER smallint not null,
	BK_TARGET_TYPE tinyint not null, -- byte; 0 = WIN, 1 = FTP, 2 = SSH, 3 = SecureCopy
	BK_SOURCE varchar(446) not null,
	BK_DESTINATION varchar(446) not null,
);

CREATE TABLE esbk_tbLogs(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- musí se vázat ke klientovi
	IDesbk_tbBackups bigint, -- long? - nemusí se vázat k záloze
	LG_TYPE tinyint not null, -- byte; 0 = error, 1 = warning; 2 = message
	LG_TIME_UTC datetime not null, -- datetime
	LG_VALUE varchar(max) not null -- string
);

BEGIN /* PK */
	ALTER TABLE esbk_tbAdministrators ADD CONSTRAINT PK_esbk_tbAdministrators_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT PK_esbk_tbEmails_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbClients ADD CONSTRAINT PK_esbk_tbClients_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT PK_esbk_tbLogins_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT PK_esbk_tbBackups_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT PK_esbk_tbBackupTemplates_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupTemplatesPaths ADD CONSTRAINT PK_esbk_tbBackupTemplatesPaths_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT PK_esbk_tbLogs_ID PRIMARY KEY NONCLUSTERED (ID);
END
BEGIN /* FK */
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT FK_esbk_tbEmails_IDesbk_tbAdministrators FOREIGN KEY (IDesbk_tbAdministrators) REFERENCES esbk_tbAdministrators(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbClients ADD CONSTRAINT FK_esbk_tbClients_IDesbk_tbAdministrators FOREIGN KEY (IDesbk_tbAdministrators) REFERENCES esbk_tbAdministrators(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT FK_esbk_tbLogins_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_IDesbk_tbBackups_BASE FOREIGN KEY (IDesbk_tbBackups_BASE) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT FK_esbk_tbBackupTemplates_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackupTemplatesPaths ADD CONSTRAINT FK_esbk_tbBackupTemplatesPaths_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID); -- zůstanou logy, které nepatří k backupu - klienti se tak často odstraňovat nebudou
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID) ON UPDATE CASCADE ON DELETE CASCADE;
END
BEGIN /* IX */
	CREATE INDEX IX_esbk_tbEmails_IDesbk_tbAdministrators ON esbk_tbEmails(IDesbk_tbAdministrators);
	CREATE UNIQUE INDEX IX_UQ_esbk_tbEmails ON esbk_tbEmails(EMAIL);
	CREATE INDEX IX_esbk_tbClients_IDesbk_tbAdministrators ON esbk_tbClients(IDesbk_tbAdministrators);
	CREATE UNIQUE INDEX IX_UQ_esbk_tbClients ON esbk_tbClients(CL_LOGIN_NAME) WHERE CL_LOGIN_NAME IS NOT NULL;
	CREATE INDEX IX_esbk_tbLogins_ID ON esbk_tbLogins(ID);
	CREATE INDEX IX_esbk_tbLogins_IDesbk_tbClients ON esbk_tbLogins(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbLogins_LG_TIME_UTC ON esbk_tbLogins(LG_TIME_UTC);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbClients ON esbk_tbBackups(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbBackupTemplates ON esbk_tbBackups(IDesbk_tbBackupTemplates);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbBackups_BASE ON esbk_tbBackups(IDesbk_tbBackups_BASE) WHERE BK_TYPE IN (1, 2);
	CREATE INDEX IX_esbk_tbBackups_BK_TIME_BEGIN_UTC ON esbk_tbBackups(BK_TIME_BEGIN_UTC);
	CREATE INDEX IX_esbk_tbBackupTemplates_IDesbk_tbClients ON esbk_tbBackupTemplates(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackupTemplatesPaths_IDesbk_tbBackupTemplates ON esbk_tbBackupTemplatesPaths(IDesbk_tbBackupTemplates);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbClients ON esbk_tbLogs(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbBackups ON esbk_tbLogs(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbLogs_LG_TYPE ON esbk_tbLogs(LG_TYPE);
	CREATE INDEX IX_esbk_tbLogs_LG_TIME_UTC ON esbk_tbLogs(LG_TIME_UTC);	
END
BEGIN /* DF */
	ALTER TABLE esbk_tbAdministrators ADD CONSTRAINT DF_esbk_tbAdministrators_AD_META_REGISTRATION_DATE_UTC DEFAULT (GETUTCDATE()) FOR AD_META_REGISTRATION_DATE_UTC;
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT DF_esbk_tbEmails_ID DEFAULT (NEWID()) FOR ID;
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT DF_esbk_tbEmails_ISDEFAULT DEFAULT (0) FOR ISDEFAULT;
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_STATUS DEFAULT (0) FOR CL_STATUS;
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_AUTO_STATUS_REPORT_ENABLED DEFAULT (1) FOR CL_AUTO_STATUS_REPORT_ENABLED;
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_META_REGISTRATION_DATE_UTC DEFAULT (GETUTCDATE()) FOR CL_META_REGISTRATION_DATE_UTC;
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_AUTO_STATUS_REPORT_INTERVAL DEFAULT (10000) FOR CL_AUTO_STATUS_REPORT_INTERVAL;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT DF_esbk_tbLogins_ID DEFAULT (NEWID()) FOR ID;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT DF_esbk_tbLogins_LG_TIME_UTC DEFAULT (GETUTCDATE()) FOR LG_TIME_UTC;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT DF_esbk_tbLogins_LG_TIME_EXPIRATION_UTC DEFAULT (DATEADD(minute, 15, GETUTCDATE())) FOR LG_TIME_EXPIRATION_UTC;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TYPE DEFAULT (0) FOR BK_TYPE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_COMPRESSION DEFAULT (0) FOR BK_COMPRESSION;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TIME_BEGIN_UTC DEFAULT (GETUTCDATE()) FOR BK_TIME_BEGIN_UTC;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_STATUS DEFAULT (0) FOR BK_STATUS;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_META_EMAIL_SENT DEFAULT (0) FOR BK_META_EMAIL_SENT;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_TYPE DEFAULT (0) FOR BK_TYPE;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_COMPRESSION DEFAULT (0) FOR BK_COMPRESSION;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_SEARCH_PATTERN DEFAULT ('*') FOR BK_SEARCH_PATTERN;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_COPY_EMPTY_DIRS DEFAULT (1) FOR BK_COPY_EMPTY_DIRS;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_ENABLED DEFAULT (0) FOR BK_ENABLED;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_NOTIFICATION_ENABLED DEFAULT (0) FOR BK_NOTIFICATION_ENABLED;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_NOTIFICATION_EMAIL_ENABLED DEFAULT (1) FOR BK_NOTIFICATION_EMAIL_ENABLED;
	ALTER TABLE esbk_tbBackupTemplatesPaths ADD CONSTRAINT DF_esbk_tbBackupTemplatesPaths_ID DEFAULT (NEWID()) FOR ID;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT DF_esbk_tbLogs_ID DEFAULT (NEWID()) FOR ID;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT DF_esbk_tbLogs_LG_TIME_UTC DEFAULT (GETUTCDATE()) FOR LG_TIME_UTC;
END
BEGIN /* CK */
	ALTER TABLE esbk_tbAdministrators ADD CONSTRAINT CK_esbk_tbAdministrators_AD_META_REGISTRATION_DATE_UTC CHECK (AD_META_REGISTRATION_DATE_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbClients ADD CONSTRAINT CK_esbk_tbClients_CL_META_LAST_BACKUP_UTC CHECK (CL_META_LAST_BACKUP_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbClients ADD CONSTRAINT CK_esbk_tbClients_CL_META_LAST_STATUS_REPORT_UTC CHECK (CL_META_LAST_STATUS_REPORT_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbClients ADD CONSTRAINT CK_esbk_tbClients_CL_META_REGISTRATION_DATE_UTC CHECK (CL_META_REGISTRATION_DATE_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT CK_esbk_tbLogins_LG_TIME_UTC CHECK (LG_TIME_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT CK_esbk_tbLogins_LG_TIME_EXPIRATION_UTC CHECK (LG_TIME_UTC <= LG_TIME_EXPIRATION_UTC);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_EXPIRATION_UTC CHECK (BK_EXPIRATION_UTC >= GETUTCDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_BEGIN_UTC CHECK (BK_TIME_BEGIN_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT CK_esbk_tbBackups_BK_TIME_END_UTC CHECK (BK_TIME_BEGIN_UTC <= BK_TIME_END_UTC AND BK_TIME_END_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT CK_esbk_tbBackupTemplates_BK_EXPIRATION_DAYS CHECK (BK_EXPIRATION_DAYS > 0);
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT CK_esbk_tbLogs_LG_TIME_UTC CHECK (LG_TIME_UTC <= GETUTCDATE());
END
BEGIN /* UQ */
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT UQ_esbk_tbEmails_EMAIL UNIQUE (EMAIL);
	ALTER TABLE esbk_tbBackupTemplatesPaths ADD CONSTRAINT UQ_esbk_tbBackupTemplatesPaths_PATH UNIQUE (IDesbk_tbBackupTemplates, BK_SOURCE, BK_DESTINATION);
END

/*
	1x Administrator
	2x Email
	3x Client
	2x Backup template (FULL, DIFF)
	1x Backup template path
*/
BEGIN /* INSERT */
	INSERT INTO esbk_tbAdministrators (AD_FIRST_NAME, AD_LAST_NAME) VALUES ('Tomáš', 'Švejnoha');
	INSERT INTO esbk_tbEmails (IDesbk_tbAdministrators, EMAIL, ISDEFAULT) VALUES (1, 'tomas.svejnoha@gmail.com', 1);
	INSERT INTO esbk_tbEmails (IDesbk_tbAdministrators, EMAIL, ISDEFAULT) VALUES (1, 'svejnohatomas@gmail.com', 0);
	INSERT INTO esbk_tbClients (IDesbk_tbAdministrators, CL_NAME, CL_HWID, CL_STATUS, CL_LOGIN_NAME, CL_LOGIN_PSWD) VALUES (1, 'PC01', 'hwid', 0, 1, 'password');
	INSERT INTO esbk_tbClients (IDesbk_tbAdministrators, CL_NAME, CL_HWID, CL_STATUS) VALUES (1, 'PC02', 'hwid', 1);
	INSERT INTO esbk_tbClients (IDesbk_tbAdministrators, CL_NAME, CL_HWID, CL_STATUS) VALUES (1, 'PC03', 'hwid', 2);
	
	INSERT INTO esbk_tbBackupTemplates (IDesbk_tbClients, BK_TYPE, BK_ENABLED, BK_REPEAT_INTERVAL_CRON) VALUES (1, 0, 1, '0 0 * * sun'); -- At 00:00 on Sunday (every)
	INSERT INTO esbk_tbBackupTemplates (IDesbk_tbClients, BK_TYPE, BK_ENABLED, BK_REPEAT_INTERVAL_CRON) VALUES (1, 1, 1, '0 0 * * *'); -- At 00:00 every day
	
	INSERT INTO esbk_tbBackupTemplatesPaths (IDesbk_tbBackupTemplates, BK_PATH_ORDER, BK_TARGET_TYPE, BK_SOURCE, BK_DESTINATION) VALUES (1, 1, 0, 'C:\src', 'C:\dst');
END

--select * from esbk_tbClients
--select * from esbk_tbLogins
--select * from esbk_tbLogs