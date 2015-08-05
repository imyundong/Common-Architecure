--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE OVERRIDELIST
(
    TXNCODE                 VARCHAR   (10)  NOT NULL,
    BRANCHCATEGORY          INT             NOT NULL,
    TELLERROLE              INT             NOT NULL,
    OVERRIDECODE            VARCHAR   (10)  NOT NULL,
    CONSTRAINT PKC_OverrideList PRIMARY KEY (OVERRIDECODE)
);

CREATE INDEX OVERRIDELIST_PK ON OverrideList(OverrideCode)

