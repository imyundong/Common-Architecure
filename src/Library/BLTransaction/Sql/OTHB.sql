--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE OTHB
(
    BANKCODE                VARCHAR   (3)   NOT NULL,
    BRANCHCODE              VARCHAR   (3)   NOT NULL,
    BRANCHFULLNAME          VARCHAR   (50)  ,        
    BRANCHSHORTNAME         VARCHAR   (20)  ,        
    CONSTRAINT PKC_OTHB PRIMARY KEY (BRANCHCODE)
);

CREATE INDEX OTHB_PK ON OTHB(BranchCode)

