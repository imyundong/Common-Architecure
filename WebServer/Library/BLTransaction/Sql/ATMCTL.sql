--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE ATMCTL
(
    TERMID                  VARCHAR   (8)   NOT NULL,
    BRANCHID                INT             NOT NULL,
    TELRNO                  VARCHAR   (10)  NOT NULL,
    SHOPLOCATION            VARCHAR   (50)  ,        
    GROUPID                 VARCHAR   (3)   ,        
    CONSTRAINT PKC_ATMCTL PRIMARY KEY (TELRNO)
);

CREATE INDEX ATMCTL_PK ON ATMCTL(TelrNo)

