--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE USERROLECATEGORY
(
    ROLEID                  INT             NOT NULL,
    ROLEDESCRIPTION         VARCHAR   (50)  ,        
    CAPABILITY              INT             ,        
    ROLEGROUP               INT             ,        
    ROLEATTRIBUITE          VARCHAR   (3)   ,        
    CONSTRAINT PKC_UserRoleCategory PRIMARY KEY (ROLEID)
);

CREATE INDEX USERROLECATEGORY_PK ON UserRoleCategory(RoleId)

