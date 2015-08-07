--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE QUEUEITEM
(
    QUEUEID                 INT             ,        
    DESCRIPTION             VARCHAR   (MAX) ,        
    XMLDOCUMENT             VARCHAR   (MAX) ,        
    PRIORITY                INT             ,        
    DATEADDED               INT             ,        
    DATEPROCESSED           INT             ,        
    STATUS                  INT             ,        
    SENDERTELLERID          INT             ,        
    PROCESSORTELLERID       INT             ,        
    CAPABILITY              INT             ,        
    TIMEADDED               INT             ,        
    DATEEXPIRED             INT             ,        
    ORIGINALITEMID          INT             ,        
    CONSTRAINT PKC_QueueItem PRIMARY KEY ()
);

CREATE INDEX QUEUEITEM_PK ON QueueItem()

