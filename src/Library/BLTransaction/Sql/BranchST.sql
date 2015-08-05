--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE BRANCHST
(
    BANKCD                  VARCHAR   (64)  NOT NULL,
    BRANCHCD                VARCHAR   (64)  NOT NULL,
    UNIONBALANCINGSTATUS    VARCHAR   (2)   ,        
    BALANCINGSTATUS         VARCHAR   (2)   ,        
    BUSINESSDATE            VARCHAR   (8)   ,        
    REMITTANCESTATUS        VARCHAR   (2)   ,        
    FRXBALANCINGSTATUS      VARCHAR   (2)   ,        
    CONSTRAINT PKC_BranchST PRIMARY KEY (BRANCHCD)
);

CREATE INDEX BRANCHST_PK ON BranchST(BRANCHCD)

