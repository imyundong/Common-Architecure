--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE OVERRIDECODE
(
    CODE                    VARCHAR   (10)  NOT NULL,
    OVERRIDEDESCRIPTION     VARCHAR   (50)  ,        
    CAPABILITY              INT             ,        
    CONDITION               VARCHAR   (MAX) ,        
    CONSTRAINT PKC_OverrideCode PRIMARY KEY (CODE)
);

CREATE INDEX OVERRIDECODE_PK ON OverrideCode(Code)

