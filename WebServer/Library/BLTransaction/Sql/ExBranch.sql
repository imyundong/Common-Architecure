--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE EXBRANCH
(
    EXBRANCHNO              INT             NOT NULL,
    EXBRANCHNAME            VARCHAR   (MAX) ,        
    CONSTRAINT PKC_ExBranch PRIMARY KEY (EXBRANCHNO)
);

CREATE INDEX EXBRANCH_PK ON ExBranch(ExBranchNo)

