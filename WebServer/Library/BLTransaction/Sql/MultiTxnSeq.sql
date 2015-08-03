--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE MULTITXNSEQ
(
    BANKCD                  VARCHAR   (64)  NOT NULL,
    BRANCHCD                VARCHAR   (64)  NOT NULL,
    BUSINESSDATE            DATETIME        NOT NULL,
    MULTISEQ                INT             NOT NULL,
    EJSEQ                   INT             ,        
    DRAMT                   FLOAT           ,        
    CRAMT                   FLOAT           ,        
    TXNCD                   VARCHAR   (8)   ,        
    USERID                  VARCHAR   (10)  ,        
    TOACCTNO                VARCHAR   (20)  ,        
    CONSTRAINT PKC_MultiTxnSeq PRIMARY KEY (MULTISEQ)
);

CREATE INDEX MULTITXNSEQ_PK ON MultiTxnSeq(MULTISEQ)

