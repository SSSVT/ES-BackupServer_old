USE [master];
IF EXISTS(SELECT * FROM sys.databases WHERE name='dbESBackup')
BEGIN
	ALTER DATABASE [dbESBackup] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [dbESBackup];
END

CREATE DATABASE [dbESBackup];
GO

USE [dbESBackup];
GO
CREATE SCHEMA [config];
GO
CREATE SCHEMA [customers];
GO
CREATE SCHEMA [endpoints];
GO

BEGIN /* Server configuration */
	CREATE TABLE [config].[esbk_MailConfig](
		ID int identity(1,1) not null,
		CFG_SERVER nvarchar(128) not null,
		CFG_PORT int not null,
		CFG_USERNAME nvarchar(128) not null,
		CFG_PASSWORD nvarchar(512) not null,
		CFG_FROM nvarchar(256) not null,
		CFG_METHOD int not null,
		CFG_PROTOCOL int not null,

		CFG_DEFAULT bit not null
	);
	ALTER TABLE [config].[esbk_MailConfig] ADD CONSTRAINT PK_esbk_MailConfig_ID PRIMARY KEY (ID);
	CREATE UNIQUE INDEX IX_esbk_MailConfig_CFG_DEFAULT ON esbk_MailConfig(CFG_DEFAULT) WHERE CFG_DEFAULT = 1; -- vynucení pouze jednoho default záznamu?
END

BEGIN /* Customers */
	CREATE TABLE [customers].[esbk_tbCustomers](
		ID bigint identity(1,1) not null,

		CST_FIRST_NAME nvarchar(50) not null,
		CST_LAST_NAME nvarchar(50) not null,

		CST_LOGIN_NAME nvarchar(128) not null, -- username
		CST_LOGIN_PSWD nvarchar(2048) not null, -- password

		CST_META_REGISTRATION_DATE_UTC datetime not null
	);
	ALTER TABLE esbk_tbCustomers ADD CONSTRAINT PK_esbk_tbCustomers_ID PRIMARY KEY (ID);

	CREATE TABLE [customers].[esbk_tbEmails](
		ID uniqueidentifier not null,
		IDesbk_tbCustomers bigint not null,

		CST_EMAIL nvarchar(512) not null,
		CST_ISDEFAULT bit not null
	);
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT PK_esbk_tbEmails_ID PRIMARY KEY NONCLUSTERED (ID);
END

BEGIN /* Endpoints */
	CREATE TABLE esbk_tbEndpoints(
		ID bigint not null,
		IDesbk_tbCustomers bigint not null,
		EPT_NAME nvarchar(64) not null,
		EPT_DESCRIPTION nvarchar(512),
		EPT_MAC nvarchar(512), -- MAC

		EPT_LOGIN_NAME nvarchar(128), -- username
		EPT_LOGIN_PSWD nvarchar(2048), -- password
		-- EPT_LOGIN_SALT nvarchar(512), -- salt
	
		EPT_STATUS tinyint not null, -- byte, autorizován/request registration/banned

		EPT_AUTO_STATUS_REPORT_ENABLED bit not null,
		EPT_AUTO_STATUS_REPORT_INTERVAL int,
		EPT_META_LAST_STATUS_REPORT_UTC datetime,

		EPT_META_LAST_BACKUP_UTC datetime, -- datetime?
		EPT_META_REGISTRATION_DATE_UTC datetime not null,
	
		EPT_META_LAST_CONFIG_UPDATE datetime,
	); /* Tabulka serverů (klientů), kteří se zálohují */
	CREATE TABLE esbk_tbLogins(
		ID uniqueidentifier not null, -- GUID
		IDesbk_tbEndpoints bigint not null, -- int

		EPT_TIME_UTC datetime not null, -- datetime
		EPT_TIME_EXPIRATION_UTC datetime not null, -- datetime; default - 15 minut; při dalším requestu obnovit na 15
		EPT_CLIENT_IP nvarchar(64) not null,
	); /* Historie přihlášení klientů */

	CREATE TABLE esbk_tbBackups(
		ID bigint identity(1,1) not null, -- long
		IDesbk_tbEndpoints int not null, -- int
		IDesbk_tbBackupTemplates bigint not null,

		BK_NAME nvarchar(128), -- string
		BK_DESCRIPTION nvarchar(512), -- string

		BK_TYPE tinyint not null, -- 0 = full, 1 = differential, 2 = incremental
		IDesbk_tbBackups_BASE bigint,

		BK_SOURCE nvarchar(max) not null, -- new
		BK_DESTINATION nvarchar(max) not null, -- new
	
		BK_EXPIRATION_UTC datetime, -- datetime?
		BK_COMPRESSION bit not null, -- 0 = do not compress; 1 = compress

		BK_TIME_BEGIN_UTC datetime not null, -- datetime
		BK_TIME_END_UTC datetime, -- datetime?, null - updatem
		BK_STATUS tinyint not null, -- Executing, Competed, Failed, ...

		BK_META_PATH_ORDER int not null, -- pořadí cesty
		BK_META_EMAIL_SENT bit not null
	); /* Historie provedených záloh */
	CREATE TABLE esbk_tbBackupTemplates(
		ID bigint identity(1,1) not null,
		IDesbk_tbEndpoints int not null,

		BK_NAME nvarchar(128), -- string
		BK_DESCRIPTION nvarchar(512), -- string

		BK_TYPE tinyint not null, -- 0 = full, 1 = differential, 2 = incremental

		BK_EXPIRATION_DAYS int, -- int?
		BK_COMPRESSION bit not null, -- 0 = do not compress; 1 = compress
		BK_SEARCH_PATTERN nvarchar(2048) not null,

		BK_ENABLED bit not null, -- zda je template aktivní

		BK_NOTIFICATION_ENABLED bit not null, -- notifikace na klientovi
		BK_NOTIFICATION_EMAIL_ENABLED bit not null, -- nofitikace emailem
		BK_REPEAT_INTERVAL_CRON nvarchar(256) not null, -- interval opakování

		BK_META_TMP_ID uniqueidentifier,
	);
	CREATE TABLE esbk_tbBackupTemplatesPaths(
		ID uniqueidentifier not null,
		IDesbk_tbBackupTemplates bigint not null,

		BK_USERNAME nvarchar(128),
		BK_PASSWORD nvarchar(512),

		BK_PATH_ORDER smallint not null,
		BK_TARGET_TYPE tinyint not null, -- byte; 0 = WIN, 1 = FTP, 2 = SSH, 3 = SecureCopy, ...
		BK_SOURCE nvarchar(446) not null,
		BK_DESTINATION nvarchar(446) not null,
	);

	CREATE TABLE esbk_tbLogs(
		ID uniqueidentifier not null, -- GUID
		IDesbk_tbEndpoints int not null, -- musí se vázat ke klientovi
		IDesbk_tbBackups bigint, -- long? - nemusí se vázat k záloze
		LG_TYPE tinyint not null, -- byte; 0 = error, 1 = warning; 2 = message
		LG_TIME_UTC datetime not null, -- datetime
		LG_VALUE nvarchar(max) not null -- string
	);
END

BEGIN /* PK */
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT PK_esbk_tbEndpoints_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT PK_esbk_tbLogins_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT PK_esbk_tbBackups_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT PK_esbk_tbBackupTemplates_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbBackupTemplatesPaths ADD CONSTRAINT PK_esbk_tbBackupTemplatesPaths_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT PK_esbk_tbLogs_ID PRIMARY KEY NONCLUSTERED (ID);
END
BEGIN /* FK */
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT FK_esbk_tbEmails_IDesbk_tbCustomers FOREIGN KEY (IDesbk_tbCustomers) REFERENCES esbk_tbCustomers(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT FK_esbk_tbEndpoints_IDesbk_tbCustomers FOREIGN KEY (IDesbk_tbCustomers) REFERENCES esbk_tbCustomers(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbLogins ADD CONSTRAINT FK_esbk_tbLogins_IDesbk_tbEndpoints FOREIGN KEY (IDesbk_tbEndpoints) REFERENCES esbk_tbEndpoints(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbEndpoints FOREIGN KEY (IDesbk_tbEndpoints) REFERENCES esbk_tbEndpoints(ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_IDesbk_tbBackups_BASE FOREIGN KEY (IDesbk_tbBackups_BASE) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT FK_esbk_tbBackupTemplates_IDesbk_tbEndpoints FOREIGN KEY (IDesbk_tbEndpoints) REFERENCES esbk_tbEndpoints(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbBackupTemplatesPaths ADD CONSTRAINT FK_esbk_tbBackupTemplatesPaths_IDesbk_tbBackupTemplates FOREIGN KEY (IDesbk_tbBackupTemplates) REFERENCES esbk_tbBackupTemplates(ID) ON UPDATE CASCADE ON DELETE CASCADE;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbEndpoints FOREIGN KEY (IDesbk_tbEndpoints) REFERENCES esbk_tbEndpoints(ID); -- zůstanou logy, které nepatří k backupu - klienti se tak často odstraňovat nebudou
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT FK_esbk_tbLogs_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID) ON UPDATE CASCADE ON DELETE CASCADE;
END
BEGIN /* IX */
	CREATE INDEX IX_esbk_tbEmails_IDesbk_tbCustomers ON esbk_tbEmails(IDesbk_tbCustomers);
	CREATE UNIQUE INDEX IX_UQ_esbk_tbEmails ON esbk_tbEmails(EMAIL);
	CREATE INDEX IX_esbk_tbEndpoints_IDesbk_tbCustomers ON esbk_tbEndpoints(IDesbk_tbCustomers);
	CREATE UNIQUE INDEX IX_UQ_esbk_tbEndpoints ON esbk_tbEndpoints(EPT_LOGIN_NAME) WHERE EPT_LOGIN_NAME IS NOT NULL;
	CREATE INDEX IX_esbk_tbLogins_ID ON esbk_tbLogins(ID);
	CREATE INDEX IX_esbk_tbLogins_IDesbk_tbEndpoints ON esbk_tbLogins(IDesbk_tbEndpoints);
	CREATE INDEX IX_esbk_tbLogins_LG_TIME_UTC ON esbk_tbLogins(LG_TIME_UTC);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbEndpoints ON esbk_tbBackups(IDesbk_tbEndpoints);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbBackupTemplates ON esbk_tbBackups(IDesbk_tbBackupTemplates);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbBackups_BASE ON esbk_tbBackups(IDesbk_tbBackups_BASE) WHERE BK_TYPE IN (1, 2);
	CREATE INDEX IX_esbk_tbBackups_BK_TIME_BEGIN_UTC ON esbk_tbBackups(BK_TIME_BEGIN_UTC);
	CREATE INDEX IX_esbk_tbBackupTemplates_IDesbk_tbEndpoints ON esbk_tbBackupTemplates(IDesbk_tbEndpoints);
	CREATE INDEX IX_esbk_tbBackupTemplates_BK_META_TMP_ID ON esbk_tbBackupTemplates(BK_META_TMP_ID) WHERE BK_META_TMP_ID IS NOT NULL;
	CREATE INDEX IX_esbk_tbBackupTemplatesPaths_IDesbk_tbBackupTemplates ON esbk_tbBackupTemplatesPaths(IDesbk_tbBackupTemplates);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbEndpoints ON esbk_tbLogs(IDesbk_tbEndpoints);
	CREATE INDEX IX_esbk_tbLogs_IDesbk_tbBackups ON esbk_tbLogs(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbLogs_LG_TYPE ON esbk_tbLogs(LG_TYPE);
	CREATE INDEX IX_esbk_tbLogs_LG_TIME_UTC ON esbk_tbLogs(LG_TIME_UTC);	
END
BEGIN /* DF */
	ALTER TABLE esbk_tbCustomers ADD CONSTRAINT DF_esbk_tbCustomers_AD_META_REGISTRATION_DATE_UTC DEFAULT (GETUTCDATE()) FOR AD_META_REGISTRATION_DATE_UTC;
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT DF_esbk_tbEmails_ID DEFAULT (NEWID()) FOR ID;
	ALTER TABLE esbk_tbEmails ADD CONSTRAINT DF_esbk_tbEmails_ISDEFAULT DEFAULT (0) FOR ISDEFAULT;
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT DF_esbk_tbEndpoints_EPT_STATUS DEFAULT (0) FOR EPT_STATUS;
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT DF_esbk_tbEndpoints_EPT_AUTO_STATUS_REPORT_ENABLED DEFAULT (1) FOR EPT_AUTO_STATUS_REPORT_ENABLED;
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT DF_esbk_tbEndpoints_EPT_META_REGISTRATION_DATE_UTC DEFAULT (GETUTCDATE()) FOR EPT_META_REGISTRATION_DATE_UTC;
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT DF_esbk_tbEndpoints_EPT_AUTO_STATUS_REPORT_INTERVAL DEFAULT (600000) FOR EPT_AUTO_STATUS_REPORT_INTERVAL;
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
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_SEARCH_PATTERN DEFAULT ('.*') FOR BK_SEARCH_PATTERN;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_ENABLED DEFAULT (0) FOR BK_ENABLED;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_NOTIFICATION_ENABLED DEFAULT (0) FOR BK_NOTIFICATION_ENABLED;
	ALTER TABLE esbk_tbBackupTemplates ADD CONSTRAINT DF_esbk_tbBackupTemplates_BK_NOTIFICATION_EMAIL_ENABLED DEFAULT (1) FOR BK_NOTIFICATION_EMAIL_ENABLED;
	ALTER TABLE esbk_tbBackupTemplatesPaths ADD CONSTRAINT DF_esbk_tbBackupTemplatesPaths_ID DEFAULT (NEWID()) FOR ID;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT DF_esbk_tbLogs_ID DEFAULT (NEWID()) FOR ID;
	ALTER TABLE esbk_tbLogs ADD CONSTRAINT DF_esbk_tbLogs_LG_TIME_UTC DEFAULT (GETUTCDATE()) FOR LG_TIME_UTC;
END
BEGIN /* CK */
	ALTER TABLE esbk_tbCustomers ADD CONSTRAINT CK_esbk_tbCustomers_AD_META_REGISTRATION_DATE_UTC CHECK (AD_META_REGISTRATION_DATE_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT CK_esbk_tbEndpoints_EPT_META_LAST_BACKUP_UTC CHECK (EPT_META_LAST_BACKUP_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT CK_esbk_tbEndpoints_EPT_META_LAST_STATUS_REPORT_UTC CHECK (EPT_META_LAST_STATUS_REPORT_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT CK_esbk_tbEndpoints_EPT_META_REGISTRATION_DATE_UTC CHECK (EPT_META_REGISTRATION_DATE_UTC <= GETUTCDATE());
	ALTER TABLE esbk_tbEndpoints ADD CONSTRAINT CK_esbk_tbEndpoints_EPT_META_LAST_CONFIG_UPDATE CHECK (EPT_META_LAST_CONFIG_UPDATE <= GETUTCDATE());
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
	INSERT INTO esbk_tbCustomers (AD_FIRST_NAME, AD_LAST_NAME, AD_LOGIN_NAME, AD_LOGIN_PSWD) VALUES ('Tomáš', 'Švejnoha', 'root', 'root');
	INSERT INTO esbk_tbEmails (IDesbk_tbCustomers, EMAIL, ISDEFAULT) VALUES (1, 'tomas.svejnoha@gmail.com', 1);
	INSERT INTO esbk_tbEmails (IDesbk_tbCustomers, EMAIL, ISDEFAULT) VALUES (1, 'svejnohatomas@gmail.com', 0);
	INSERT INTO esbk_tbEndpoints (IDesbk_tbCustomers, EPT_NAME, EPT_HWID, EPT_STATUS, EPT_LOGIN_NAME, EPT_LOGIN_PSWD) VALUES (1, 'PC01', 'hwid', 0, 1, 'password');
	INSERT INTO esbk_tbEndpoints (IDesbk_tbCustomers, EPT_NAME, EPT_HWID, EPT_STATUS) VALUES (1, 'PC02', 'hwid', 1);
	INSERT INTO esbk_tbEndpoints (IDesbk_tbCustomers, EPT_NAME, EPT_HWID, EPT_STATUS) VALUES (1, 'PC03', 'hwid', 2);
	
	INSERT INTO esbk_tbBackupTemplates (IDesbk_tbEndpoints, BK_TYPE, BK_ENABLED, BK_REPEAT_INTERVAL_CRON) VALUES (1, 0, 1, '0 0 * * sun'); -- At 00:00 on Sunday (every)
	INSERT INTO esbk_tbBackupTemplates (IDesbk_tbEndpoints, BK_TYPE, BK_ENABLED, BK_REPEAT_INTERVAL_CRON) VALUES (1, 1, 1, '0 0 * * *'); -- At 00:00 every day
	
	INSERT INTO esbk_tbBackupTemplatesPaths (IDesbk_tbBackupTemplates, BK_PATH_ORDER, BK_TARGET_TYPE, BK_SOURCE, BK_DESTINATION) VALUES (1, 1, 0, 'C:\src', 'C:\dst');

	INSERT INTO esbk_tbBackups VALUES (1, 1, 'Test', 'Test', 0, NULL, 'C:\src', 'C:\dst', NULL, 0, GETUTCDATE(), NULL, 1, 1,0)
	INSERT INTO esbk_tbBackups VALUES (1, 1, 'Test', 'Test', 0, NULL, 'C:\src', 'C:\dst2', NULL, 0, GETUTCDATE(), NULL, 1, 1,0)
	INSERT INTO esbk_tbBackups VALUES (1, 1, 'Test', 'Test', 0, NULL, 'C:\src', 'C:\dst3', NULL, 0, GETUTCDATE(), NULL, 1, 1,0)

	INSERT INTO esbk_MailConfig VALUES ('smtp.seznam.cz',465,'backuptesting@seznam.cz','evolutionstudio','evolutionstudio@report.com',0,48,1)
END

--select * from esbk_tbEndpoints
--select * from esbk_tbLogins
--select * from esbk_tbLogs