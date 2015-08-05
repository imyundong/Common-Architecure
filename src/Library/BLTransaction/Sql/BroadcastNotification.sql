--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE BROADCASTNOTIFICATION
(
    TELLERID                VARCHAR   (8)   ,        
    SENDER                  INT             ,        
    SYSTEMDATE              DATETIME        ,        
    CONSTRAINT PKC_BroadcastNotification PRIMARY KEY ()
);

CREATE INDEX BROADCASTNOTIFICATION_PK ON BroadcastNotification()

