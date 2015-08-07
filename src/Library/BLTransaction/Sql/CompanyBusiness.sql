--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE COMPANYBUSINESS
(
    PAYROLLCODE             VARCHAR   (5)   NOT NULL,
    COMPANYID               VARCHAR   (10)  ,        
    COMPANYNAME             VARCHAR   (200) ,        
    TRANSACTIONTYPE         VARCHAR   (3)   ,        
    CONSTRAINT PKC_CompanyBusiness PRIMARY KEY (PAYROLLCODE)
);

CREATE INDEX COMPANYBUSINESS_PK ON CompanyBusiness(PayrollCode)

