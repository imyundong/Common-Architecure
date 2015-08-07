--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE BGLACCOUNTPR
(
    BRANCHCD                VARCHAR   (64)  NOT NULL,
    ACCOUNT                 VARCHAR   (16)  NOT NULL,
    CANELMARK               CHAR      (1)   ,        
    ZEROMARK                CHAR      (1)   ,        
    CONSTRAINT PKC_BGLACCOUNTPR PRIMARY KEY (ACCOUNT)
);

CREATE INDEX BGLACCOUNTPR_PK ON BGLACCOUNTPR(ACCOUNT)

