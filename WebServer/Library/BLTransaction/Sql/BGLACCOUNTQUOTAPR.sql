--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE BGLACCOUNTQUOTAPR
(
    TITLEID                 VARCHAR   (10)  NOT NULL,
    LIMITAMOUNT             INT             ,        
    CONSTRAINT PKC_BGLACCOUNTQUOTAPR PRIMARY KEY (TITLEID)
);

CREATE INDEX BGLACCOUNTQUOTAPR_PK ON BGLACCOUNTQUOTAPR(TITLEID)

