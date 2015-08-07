--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE MARQUEE
(
    SYSTEMID                INT             NOT NULL,
    CONTENT                 VARCHAR   (50)  ,        
    CONSTRAINT PKC_Marquee PRIMARY KEY (SYSTEMID)
);

CREATE INDEX MARQUEE_PK ON Marquee(SystemId)

