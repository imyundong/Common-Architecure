--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE HOSTS
(
    HOSTID                  INT             NOT NULL,
    HOSTNAME                VARCHAR   (50)  ,        
    CONSTRAINT PKC_Hosts PRIMARY KEY (HOSTID)
);

CREATE INDEX HOSTS_PK ON Hosts(HostId)

