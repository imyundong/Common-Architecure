--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE USERPLUGIN
(
    USERID                  INT             NOT NULL,
    PLUGINID                INT             NOT NULL,
    CONSTRAINT PKC_UserPlugin PRIMARY KEY (PLUGINID)
);

CREATE INDEX USERPLUGIN_PK ON UserPlugin(PluginId)

