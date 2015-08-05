--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE JOURNALVIEW
(
    JOURNALID               INT             ,        
    GROUPID                 VARCHAR   (20)  ,        
    TXNCODE                 VARCHAR   (10)  ,        
    TITLE                   VARCHAR   (10)  ,        
    STATUS                  INT             ,        
    STATUSDESCRIPTION       VARCHAR   (10)  ,        
    ERRCODE                 VARCHAR   (10)  ,        
    ERRDESCRIPTION          VARCHAR   (10)  ,        
    PROCTIME                VARCHAR   (10)  ,        
    HOSTID                  INT             ,        
    HOSTNAME                VARCHAR   (10)  ,        
    TRACENO                 INT             ,        
    SYSTEMDATE              DATETIME        ,        
    BUSINESSDATE            DATETIME        ,        
    TELLER                  INT             ,        
    TELLERNAME              VARCHAR   (10)  ,        
    BRANCHID                INT             ,        
    SUPERVISOR              INT             ,        
    SUPERVISORNAME          VARCHAR   (10)  ,        
    ACCOUNT                 VARCHAR   (18)  ,        
    CURRENCY                VARCHAR   (10)  ,        
    TXNAMOUNT               FLOAT           ,        
    TERMINAL                INT             ,        
    PAGEDATA                VARCHAR   (MAX) ,        
    OVERRIDEID              VARCHAR   (50)  ,        
    REVERSALTXN             VARCHAR   (10)  ,        
    CONSTRAINT PKC_JournalView PRIMARY KEY ()
);

CREATE INDEX JOURNALVIEW_PK ON JournalView()

