/* INSERT VALUES */

use dbESBackup;

--insert into esbk_tbClients (CL_NAME, CL_LOGIN_NAME, CL_LOGIN_PSWD, CL_LOGIN_SALT, CL_VERIFIED) values ('SRV-TEST', 'test', 'test', 'salt', 1);
--select * from esbk_tbClients

--insert into esbk_tbBackupTemplates (IDesbk_tbClients, BK_SOURCE, BK_DESTINATION, BK_TYPE, BK_COMPRESSION) values (1, 'C:\', 'D:\', 0, 0);
--select * from esbk_tbBackupTemplates