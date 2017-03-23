/* INFO
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

	-- info o backupech (kvůli selectům a nastavení) - 
	--CL_LAST_BACKUP datetime, -- nemusel ještě proběhnout žádný
	--CL_BACKUP_AUTOREPEAT_TIME bigint, -- časová prodleva mezi backupy (v minutách)
	--CL_BACKUP_ATTEMPTS int not null,
	

	CL_VERIFIED bit not null -- bool, autorizován
); /* Tabulka serverů (klientů), kteří se zálohují */
CREATE TABLE esbk_tbClientLogins(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int

	LG_TIME datetime not null, -- datetime
	LG_CLIENT_IP varbinary(128) not null, -- Byte[]; IPv4 - 32 bitů, IPv6 - 128 bitů
); /* Historie přihlášení klientů */

CREATE TABLE esbk_tbBackups(
	ID bigint identity(1,1) not null, -- long
	IDesbk_tbClients int not null, -- int

	BK_NAME varchar(128), -- string
	BK_DESCRIPTION varchar(512), -- string
	
	BK_TIME_BEGIN datetime not null, -- datetime
	BK_TIME_END datetime, -- datetime?, null - updatem

	BK_EXPIRATION datetime, -- datetime?
	BK_COMPRESSION char(1) -- C = compress, N = do not compress
); /* Historie provedených záloh */
CREATE TABLE esbk_tbBackupDetails(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbBackups bigint not null, -- long
	BK_PATH_SOURCE varchar(max), -- string
	BK_PATH_TARGET varchar(max), -- string

	-- k ověření
	BK_TIME datetime not null, -- datetime, čas provedení zálohy
	BK_LAST_CHANGE datetime not null, -- datetime - získání ze souboru
	BK_HASH varchar(2048), -- string
); /* Info o jednotlivých zálohách (tbRozpisObjednávek) */

CREATE TABLE esbk_tbBackupSetting(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- int
	IDesbk_tbBackupSettingTypes int not null, -- int
	BK_VALUE varchar(4096) not null -- string
); /* Nastavení zálohování pro klienty - odkud/kam, výjimky/pouze (cesta, *.pdf, *.docx, ...) */
CREATE TABLE esbk_tbBackupSettingTypes(
	ID int identity(1,1) not null,
	TP_NAME varchar(64) not null
); /* Typy nastavení */

CREATE TABLE esbk_tbClientsActions(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null,
	IDesbk_tbClientActionTypes int not null,

	AC_VALUE varchar(max), -- parameters (path src=C: dst=D:, email, ...)

	-- TIME/BEFORE/AFTER BACKUP
	AC_ACTION_TYPE bit not null, -- 0 = event (before/after); 1 = time
	AC_EVENT bit, -- 0 = before backup, 1 = after backup
	AC_TIME datetime, -- datetime?
); /* Akce vázané na klienty a jejich nastavení */
CREATE TABLE esbk_tbClientActionTypes(
	ID int identity(1,1) not null, -- int
	TP_NAME varchar(64) not null
); /* Typy akcí */
CREATE TABLE esbk_tbClientLogs(
	ID uniqueidentifier not null, -- GUID
	IDesbk_tbClients int not null, -- musí se vázat ke klientovi
	IDesbk_tbBackups bigint, -- long? - nemusí se vázat k záloze
	IDesbk_tbClientLogTypes tinyint not null, -- byte
	LG_TIME datetime not null, -- datetime
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
	ALTER TABLE esbk_tbBackupSetting ADD CONSTRAINT PK_esbk_tbBackupSetting_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbBackupSettingTypes ADD CONSTRAINT PK_esbk_tbBackupSettingTypes_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbClientsActions ADD CONSTRAINT PK_esbk_tbClientsActions_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbClientActionTypes ADD CONSTRAINT PK_esbk_tbClientActionTypes_ID PRIMARY KEY (ID);
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT PK_esbk_tbClientLogs_ID PRIMARY KEY NONCLUSTERED (ID);
	ALTER TABLE esbk_tbClientLogTypes ADD CONSTRAINT PK_esbk_tbClientLogTypes_ID PRIMARY KEY (ID);
END
BEGIN /* FK */
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT FK_esbk_tbClientLogins_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT FK_esbk_tbBackups_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT FK_esbk_tbBackupDetails_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbBackupSetting ADD CONSTRAINT FK_esbk_tbBackupSetting_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbBackupSetting ADD CONSTRAINT FK_esbk_tbBackupSetting_IDesbk_tbBackupSettingTypes FOREIGN KEY (IDesbk_tbBackupSettingTypes) REFERENCES esbk_tbBackupSettingTypes(ID);
	ALTER TABLE esbk_tbClientsActions ADD CONSTRAINT FK_esbk_tbClientsActions_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbClientsActions ADD CONSTRAINT FK_esbk_tbClientsActions_IDesbk_tbClientActionTypes FOREIGN KEY (IDesbk_tbClientActionTypes) REFERENCES esbk_tbClientActionTypes(ID);

	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT FK_esbk_tbClientLogs_IDesbk_tbClients FOREIGN KEY (IDesbk_tbClients) REFERENCES esbk_tbClients(ID);
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT FK_esbk_tbClientLogs_IDesbk_tbBackups FOREIGN KEY (IDesbk_tbBackups) REFERENCES esbk_tbBackups(ID);
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT FK_esbk_tbClientLogs_IDesbk_tbClientLogTypes FOREIGN KEY (IDesbk_tbClientLogTypes) REFERENCES esbk_tbClientLogTypes(ID);
END
BEGIN /* IX */
	CREATE INDEX IX_esbk_tbClientLogins_IDesbk_tbClients ON esbk_tbClientLogins(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackups_IDesbk_tbClients ON esbk_tbBackups(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackupDetails_IDesbk_tbBackups ON esbk_tbBackupDetails(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbBackupSetting_IDesbk_tbClients ON esbk_tbBackupSetting(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbBackupSetting_IDesbk_tbBackupSettingTypes ON esbk_tbBackupSetting(IDesbk_tbBackupSettingTypes);
	CREATE INDEX IX_esbk_tbClientsActions_IDesbk_tbClients ON esbk_tbClientsActions(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbClientsActions_IDesbk_tbClientActionTypes ON esbk_tbClientsActions(IDesbk_tbClientActionTypes);

	CREATE INDEX IX_esbk_tbClientLogs_IDesbk_tbClients ON esbk_tbClientLogs(IDesbk_tbClients);
	CREATE INDEX IX_esbk_tbClientLogs_IDesbk_tbBackups ON esbk_tbClientLogs(IDesbk_tbBackups);
	CREATE INDEX IX_esbk_tbClientLogs_IDesbk_tbClientLogTypes ON esbk_tbClientLogs(IDesbk_tbClientLogTypes);

END
BEGIN /* DF */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_VERIFIED DEFAULT (0) FOR CL_VERIFIED;
	ALTER TABLE esbk_tbClients ADD CONSTRAINT DF_esbk_tbClients_CL_BACKUP_ATTEMPTS DEFAULT (0) FOR CL_BACKUP_ATTEMPTS;
	ALTER TABLE esbk_tbClientLogins ADD CONSTRAINT DF_esbk_tbClientLogins_LG_TIME DEFAULT (GETDATE()) FOR LG_TIME;
	ALTER TABLE esbk_tbBackups ADD CONSTRAINT DF_esbk_tbBackups_BK_TIME_BEGIN DEFAULT (GETDATE()) FOR BK_TIME_BEGIN;
	ALTER TABLE esbk_tbBackupDetails ADD CONSTRAINT DF_esbk_tbBackupDetails_BK_TIME DEFAULT (GETDATE()) FOR BK_TIME;
	ALTER TABLE esbk_tbClientLogs ADD CONSTRAINT DF_esbk_tbClientLogs_LG_TIME DEFAULT (GETDATE()) FOR LG_TIME;
END
BEGIN /* CK */
	ALTER TABLE esbk_tbClients ADD CONSTRAINT CK_esbk_tbClients_CL_BACKUP_ATTEMPTS CHECK (CL_BACKUP_ATTEMPTS >= 0);
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
	ALTER TABLE esbk_tbBackupSettingTypes ADD CONSTRAINT UQ_esbk_tbBackupSettingTypes_TP_NAME UNIQUE (TP_NAME);
	ALTER TABLE esbk_tbClientActionTypes ADD CONSTRAINT UQ_esbk_tbClientActionTypes_TP_NAME UNIQUE (TP_NAME);
	ALTER TABLE esbk_tbClientLogTypes ADD CONSTRAINT UQ_esbk_tbClientLogTypes_TP_NAME UNIQUE (TP_NAME);
END

BEGIN /* INSERT INTO esbk_tbBackupSettingTypes */
	INSERT INTO esbk_tbBackupSettingTypes VALUES ('IGNORE'); -- ignorovat cestu, soubory, ...
	INSERT INTO esbk_tbBackupSettingTypes VALUES ('ONLY'); -- pouze cestu, soubory, ...
	INSERT INTO esbk_tbBackupSettingTypes VALUES ('PATH SOURCE'); -- cesta zdroj
	INSERT INTO esbk_tbBackupSettingTypes VALUES ('PATH TARGET'); -- cesta cíl
END
BEGIN /* INSERT INTO esbk_tbClientActionTypes */
	INSERT INTO esbk_tbClientActionTypes VALUES ('SHUTDOWN');
	INSERT INTO esbk_tbClientActionTypes VALUES ('RESTART');
	INSERT INTO esbk_tbClientActionTypes VALUES ('SLEEP');
	INSERT INTO esbk_tbClientActionTypes VALUES ('HIBERNATE');
	INSERT INTO esbk_tbClientActionTypes VALUES ('LOCK');

	INSERT INTO esbk_tbClientActionTypes VALUES ('EMAIL');
	INSERT INTO esbk_tbClientActionTypes VALUES ('NOTIFICATION'); -- notifikace na obrazovce
END
BEGIN /* INSERT INTO esbk_tbClientLogTypes */
	INSERT INTO esbk_tbClientLogTypes VALUES ('ERROR'); -- exceptions
	INSERT INTO esbk_tbClientLogTypes VALUES ('WARNING'); -- nevím, ale může se hodit :D
	INSERT INTO esbk_tbClientLogTypes VALUES ('MESSAGE'); -- finished, email sent, ...
END