--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE PLUGINLIST
(
    PLUGINID                INT             NOT NULL,
    PLUGINNAME              VARCHAR   (50)  ,        
    PLUGINFRIENDLYNAME      VARCHAR   (50)  ,        
    PLUGINICON              INT             ,        
    CONSTRAINT PKC_PluginList PRIMARY KEY (PLUGINID)
);

CREATE INDEX PLUGINLIST_PK ON PluginList(PluginId)

