--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE BROKERPR
(
    BRANCHCD                VARCHAR   (64)  NOT NULL,
    BROKERCD                VARCHAR   (64)  NOT NULL,
    BROKERNAME              VARCHAR   (200) ,        
    CONSTRAINT PKC_BrokerPr PRIMARY KEY (BROKERCD)
);

CREATE INDEX BROKERPR_PK ON BrokerPr(brokercd)

