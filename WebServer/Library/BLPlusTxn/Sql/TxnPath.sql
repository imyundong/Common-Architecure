--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE TXNPATH
(
    TXNPATHID               INT             NOT NULL,
    TXNPATHDESCRIPTION      VARCHAR   (50)  ,        
    PARENT                  INT             ,        
    ICONINDEX               INT             ,        
    PRIORITY                INT             ,        
    CONSTRAINT PKC_TxnPath PRIMARY KEY (TXNPATHID)
);

CREATE INDEX TXNPATH_PK ON TxnPath(TxnPathId)

