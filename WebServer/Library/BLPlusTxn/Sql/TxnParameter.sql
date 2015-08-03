--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE TXNPARAMETER
(
    TXNCODE                 VARCHAR   (10)  NOT NULL,
    DESCRIPTION             VARCHAR   (50)  ,        
    JOURNALISE              INT             ,        
    HOST                    INT             ,        
    ISREVERSAL              INT             ,        
    REVERSALTXN             VARCHAR   (10)  ,        
    TXNPATH                 INT             ,        
    TXNICON                 INT             ,        
    HOSTTXNCODE             VARCHAR   (10)  ,        
    CONSTRAINT PKC_TxnParameter PRIMARY KEY (TXNCODE)
);

CREATE INDEX TXNPARAMETER_PK ON TxnParameter(TxnCode)

