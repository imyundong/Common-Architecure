--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE TELLER
(
    TELLERID                VARCHAR   (8)   NOT NULL,
    BRANCHID                INT             ,        
    ENABLED                 INT             ,        
    TELLERNAME              VARCHAR   (MAX) ,        
    TERMINALID              INT             ,        
    CAPABILITY              INT             ,        
    HOSTTELLERID            INT             ,        
    LEAVEDATE               VARCHAR   (8)   ,        
    CONSTRAINT PKC_Teller PRIMARY KEY (TELLERID)
);

CREATE INDEX TELLER_PK ON Teller(TellerId)

