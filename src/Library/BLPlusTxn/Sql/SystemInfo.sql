--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE SYSTEMINFO
(
    SYSTEMID                INT             NOT NULL,
    SYSTEMNAME              VARCHAR   (50)  ,        
    VERSION                 VARCHAR   (50)  ,        
    CONSTRAINT PKC_SystemInfo PRIMARY KEY (SYSTEMID)
);

CREATE INDEX SYSTEMINFO_PK ON SystemInfo(SystemId)

