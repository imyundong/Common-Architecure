--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE SIGNONSUPERVISOR
(
    TELLERID                VARCHAR   (8)   NOT NULL,
    BRANCHID                INT             ,        
    CURRENTBRANCH           INT             ,        
    TELLERROLEID            INT             ,        
    ENABLED                 INT             ,        
    TELLERNAME              VARCHAR   (50)  ,        
    CONSTRAINT PKC_SignOnSupervisor PRIMARY KEY (TELLERID)
);

CREATE INDEX SIGNONSUPERVISOR_PK ON SignOnSupervisor(TellerId)

