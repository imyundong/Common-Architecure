--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE COMPANYNO
(
    PAYROLLCODE             VARCHAR   (5)   NOT NULL,
    COMPANYID               VARCHAR   (10)  ,        
    COMPANYNAME             VARCHAR   (100) ,        
    CONSTRAINT PKC_COMPANYNO PRIMARY KEY (PAYROLLCODE)
);

CREATE INDEX COMPANYNO_PK ON COMPANYNO(PAYROLLCODE)

