--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE HOSTTELLERMAPPING
(
    USERID                  INT             NOT NULL,
    HOSTID                  INT             NOT NULL,
    HOSTTELLER              VARCHAR   (16)  ,        
    CONSTRAINT PKC_HostTellerMapping PRIMARY KEY (HOSTID)
);

CREATE INDEX HOSTTELLERMAPPING_PK ON HostTellerMapping(HostId)

