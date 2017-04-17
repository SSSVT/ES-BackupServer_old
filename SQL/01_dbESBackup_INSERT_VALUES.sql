/* INSERT VALUES */

use dbESBackup;

insert into esbk_tbClients (CL_NAME,CL_EMAILS, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_STATUS,CL_AUTO_STATUS_REPORT_ENABLED) values ('SRV-Pepa','spravce@gmail.com', 'Hl. admin', 'test', 'salt', 0,0);
insert into esbk_tbClients (CL_NAME,CL_EMAILS, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_STATUS,CL_AUTO_STATUS_REPORT_ENABLED) values ('SRV-Franta','skladnik@gmail.com;frantik@seznam.cz', 'Skladnik', 'test', 'salt', 0,0);
insert into esbk_tbClients (CL_NAME,CL_EMAILS, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_STATUS,CL_AUTO_STATUS_REPORT_ENABLED) values ('ST-PC_STANICE','stanicnipc@email.cz', 'PC v interieru', 'test', 'salt', 1,0);
insert into esbk_tbClients (CL_NAME,CL_EMAILS, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_STATUS,CL_AUTO_STATUS_REPORT_ENABLED) values ('ST-PC_Sklad','sklad@evostudio.cz;skladprivate@evostudio.cz;skladfiremni@evostudio.cz', 'PC ve skladu', 'test', 'salt', 2,0);
select * from esbk_tbClients

insert into esbk_tbBackupTemplates (IDesbk_tbClients,BK_NAME,BK_TYPE, BK_COMPRESSION) values (1,'Full Backup Immediate', 0, 0);
insert into esbk_tbBackupTemplates (IDesbk_tbClients,BK_NAME, BK_TYPE, BK_COMPRESSION, BK_ENABLED) values (2,'Full Backup Every Sunday', 0, 0,1);
select * from esbk_tbBackupTemplates

insert into esbk_tbBackups VALUES (1,1,'Hlavní backup','Záloha disku C:','C:\','D:\',0,null,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-10,GETDATE()),0)
insert into esbk_tbBackups VALUES (1,1,'Zalozni backup','Zaloha disku Q','Q:\','D:\Backup_Q\',0,null,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-5,GETDATE()),0)
insert into esbk_tbBackups VALUES (1,2,'Differenèní backup','Diff záloha disku C:','C:\','D:\',1,null,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-4,GETDATE()),0)
insert into esbk_tbBackups VALUES (2,2,'Hlavní stanièni backup','Záloha disku X:','X:\','Y:\',0,null,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-2,GETDATE()),0)
insert into esbk_tbBackups VALUES (3,2,'Sekundarni zaloha','Záloha disku B:','B:\','X:\Backup_disku_B\',0,null,null,0,DATEADD(HH,-15,GETDATE()),DATEADD(HH,-1,GETDATE()),0)
select * from esbk_tbBackups
