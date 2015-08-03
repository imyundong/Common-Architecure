--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE CMSKEY
(
    KEYID                   VARCHAR   (20)  NOT NULL,
    KEYVALUE                VARCHAR   (200) ,        
    CONSTRAINT PKC_CMSKEY PRIMARY KEY (KEYID)
);

CREATE INDEX CMSKEY_PK ON CMSKEY(KEYID)

