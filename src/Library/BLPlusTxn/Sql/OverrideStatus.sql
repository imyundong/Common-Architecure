--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE OVERRIDESTATUS
(
    OVERRIDESTATUSID        INT             NOT NULL,
    DESCRIPTION             VARCHAR   (50)  ,        
    CONSTRAINT PKC_OverrideStatus PRIMARY KEY (OVERRIDESTATUSID)
);

CREATE INDEX OVERRIDESTATUS_PK ON OverrideStatus(OverrideStatusId)

