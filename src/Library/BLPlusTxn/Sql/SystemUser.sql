--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE SYSTEMUSER
(
    USERID                  INT             NOT NULL,
    USERALIAS               VARCHAR   (16)  ,        
    EMAIL                   VARCHAR   (50)  ,        
    FIRSTNAME               VARCHAR   (20)  ,        
    LASTNAME                VARCHAR   (20)  ,        
    MIDDLENAME              VARCHAR   (20)  ,        
    FULLNAME                VARCHAR   (60)  ,        
    GENDER                  INT             ,        
    REGISTERDATE            DATETIME        ,        
    LASTLOGINDATE           DATETIME        ,        
    BRANCH                  INT             ,        
    PHOTO                   VARCHAR   (50)  ,        
    PINBLOCK                VARCHAR   (64)  ,        
    CONTACTNO               VARCHAR   (20)  ,        
    CONTACTNO2              VARCHAR   (20)  ,        
    NATIONALITY             VARCHAR   (2)   ,        
    CONSTRAINT PKC_SystemUser PRIMARY KEY (USERID)
);

CREATE INDEX SYSTEMUSER_PK ON SystemUser(UserId)

