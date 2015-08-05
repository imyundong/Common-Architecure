--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE TELLERSTATE
(
    TELLERID                VARCHAR   (8)   ,        
    TELLERROLEID            INT             ,        
    CURRENTBRANCH           INT             ,        
    PARENTBRANCH            INT             ,        
    CONSTRAINT PKC_TellerState PRIMARY KEY ()
);

CREATE INDEX TELLERSTATE_PK ON TellerState()

