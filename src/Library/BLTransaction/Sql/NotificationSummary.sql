--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE NOTIFICATIONSUMMARY
(
    TELLERID                VARCHAR   (8)   ,        
    IBDNOTIFICATIONCOUNT    INT             ,        
    PASSTHRUNOTIFICATIONCOUNT    INT             ,        
    BROADCASTNOTIFICATIONCOUNT    INT             ,        
    CONSTRAINT PKC_NotificationSummary PRIMARY KEY ()
);

CREATE INDEX NOTIFICATIONSUMMARY_PK ON NotificationSummary()

