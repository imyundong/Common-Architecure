--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE OVERRIDEHISTORY
(
    SEQUENCENO              IDENTITY        NOT NULL,
    OVERRIDEID              VARCHAR   (50)  ,        
    OVERRIDECODE            VARCHAR   (10)  ,        
    USERID                  INT             ,        
    SUPERVISORID            INT             ,        
    STATUS                  INT             ,        
    REQUESTDATE             DATETIME        ,        
    UPDATEDATE              DATETIME        ,        
    CONSTRAINT PKC_OverrideHistory PRIMARY KEY (SEQUENCENO)
);

CREATE INDEX OVERRIDEHISTORY_PK ON OverrideHistory(SequenceNo)

