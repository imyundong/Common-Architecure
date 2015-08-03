--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE JOURNAL
(
    JOURNALID               IDENTITY        NOT NULL,
    GROUPID                 VARCHAR   (20)  ,        
    TXNCODE                 VARCHAR   (50)  ,        
    STATUS                  INT             ,        
    ERRCODE                 VARCHAR   (50)  ,        
    ERRDESCRIPTION          VARCHAR   (50)  ,        
    HOSTID                  VARCHAR   (50)  ,        
    TRACENO                 VARCHAR   (50)  ,        
    SYSTEMDATE              DATETIME        ,        
    BUSINESSDATE            DATETIME        ,        
    USERID                  INT             ,        
    BRANCHID                INT             ,        
    SUPERVISOR              INT             ,        
    ACCOUNT                 VARCHAR   (50)  ,        
    AMOUNT                  FLOAT           ,        
    CURRENCY                VARCHAR   (50)  ,        
    REQUEST                 VARCHAR   (MAX) ,        
    RESPONSE                VARCHAR   (MAX) ,        
    TERMINAL                INT             ,        
    PROCTIME                LONG            ,        
    PAGEDATA                VARCHAR   (MAX) ,        
    OVERRIDEID              VARCHAR   (50)  ,        
    CONSTRAINT PKC_Journal PRIMARY KEY (JOURNALID)
);

CREATE INDEX JOURNAL_PK ON Journal(JournalId)

