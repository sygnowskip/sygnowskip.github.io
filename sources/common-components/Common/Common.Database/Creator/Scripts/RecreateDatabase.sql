﻿IF (EXISTS (SELECT name FROM master.dbo.sysdatabases where name = '{0}'))
BEGIN

ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE {0}
END

CREATE DATABASE {0}