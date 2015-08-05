--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE BRANCH
(
    BRANCHID                INT             NOT NULL,
    BRANCHNAME              VARCHAR   (50)  ,        
    BRANCHCATEGORY          INT             ,        
    PARENT                  INT             ,        
    PROVINCIALBRANCH        INT             ,        
    CONSTRAINT PKC_Branch PRIMARY KEY (BRANCHID)
);

CREATE INDEX BRANCH_PK ON Branch(BranchId)

