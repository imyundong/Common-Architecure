--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE DBTEST
(
    TESTID                  INT             NOT NULL,
    TESTVALUE               VARCHAR   (MAX) ,        
    TESTDATE                DATETIME        ,        
    CONSTRAINT PKC_DBTest PRIMARY KEY (TESTID)
);

CREATE INDEX DBTEST_PK ON DBTest(TestId)

