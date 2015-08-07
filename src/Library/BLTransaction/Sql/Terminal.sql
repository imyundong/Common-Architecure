--------------------------------------------------------------------------------
-- This File was Automaticly Created By DBAC Tools 
-- Create Ver : 2.1.0.0
--------------------------------------------------------------------------------

CREATE TABLE TERMINAL
(
    TERMINALID              INT             NOT NULL,
    BRANCHID                INT             ,        
    CONSTRAINT PKC_Terminal PRIMARY KEY (TERMINALID)
);

CREATE INDEX TERMINAL_PK ON Terminal(TerminalId)

