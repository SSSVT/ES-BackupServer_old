/* INSERT VALUES */

use dbESBackup;

insert into esbk_tbClients (CL_NAME, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_STATUS,CL_AUTO_STATUS_REPORT_ENABLED) values ('SRV-Pepa', 'Hl. admin', 'test', 'salt', 1,0);
insert into esbk_tbClients (CL_NAME, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_STATUS,CL_AUTO_STATUS_REPORT_ENABLED) values ('SRV-Franta', 'Skladnik', 'test', 'salt', 1,0);
insert into esbk_tbClients (CL_NAME, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_STATUS,CL_AUTO_STATUS_REPORT_ENABLED) values ('ST-PC_STANICE', 'PC v interieru', 'test', 'salt', 1,0);
insert into esbk_tbClients (CL_NAME, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_STATUS,CL_AUTO_STATUS_REPORT_ENABLED) values ('ST-PC_Sklad', 'PC ve skladu', 'test', 'salt', 1,0);
select * from esbk_tbClients

insert into esbk_tbBackups VALUES ('1','Hlavní backup','Záloha disku C:','C:\','D:\',0,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-10,GETDATE()),0)
insert into esbk_tbBackups VALUES ('1','Zalozni backup','Zaloha disku Q','Q:\','D:\Backup_Q\',0,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-5,GETDATE()),0)
insert into esbk_tbBackups VALUES ('1','Differenèní backup','Diff záloha disku C:','C:\','D:\',1,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-4,GETDATE()),0)
insert into esbk_tbBackups VALUES ('2','Hlavní stanièni backup','Záloha disku X:','X:\','Y:\',0,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-2,GETDATE()),0)
insert into esbk_tbBackups VALUES ('3','Sekundarni zaloha','Záloha disku B:','B:\','X:\Backup_disku_B\',0,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-1,GETDATE()),0)
select * from esbk_tbBackups

insert into esbk_tbBackupTemplates (IDesbk_tbClients, BK_SOURCE, BK_DESTINATION, BK_TYPE, BK_COMPRESSION) values (1, 'C:\', 'D:\', 0, 0);
select * from esbk_tbBackupTemplates