
CREATE TABLE SCOPE (
    ID VARCHAR(36) NOT NULL DEFAULT (UUID()),
    NAME VARCHAR(250) NOT NULL,
    DESCRIPTION VARCHAR(1000) NULL,
    ACTIVE BIT NOT NULL DEFAULT 1,
    CREATE_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT PK_SCOPE PRIMARY KEY (ID),
    CONSTRAINT UK_SCOPE_NAME UNIQUE (NAME)
);

-- A client that can request credentials
CREATE TABLE CLIENT (
    ID VARCHAR(36) NOT NULL DEFAULT (UUID()),
    NAME VARCHAR(250) NOT NULL,
    DESCRIPTION VARCHAR(1000) NULL,
    SECRET VARCHAR(2000) NOT NULL,
    ACTIVE BIT NOT NULL DEFAULT 1,
    CREATE_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT PK_CLIENT PRIMARY KEY (ID)
);

-- Redirect / Post Logout URIs for the client
CREATE TABLE CLIENT_URI (
    ID VARCHAR(36) NOT NULL DEFAULT (UUID()),
    CLIENT_ID VARCHAR(36) NOT NULL,
    TYPE INT NOT NULL,
    URI  VARCHAR(5000) NOT NULL,
    CREATE_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT PK_CLIENT_URI PRIMARY KEY (ID),
    CONSTRAINT FK_CLIENT_URI_CLIENT FOREIGN KEY (CLIENT_ID) REFERENCES CLIENT(ID)
);

-- The scopes the client is allowed to request
CREATE TABLE CLIENT_ALLOWED_SCOPE (
    ID VARCHAR(36) NOT NULL DEFAULT (UUID()),
    CLIENT_ID VARCHAR(36) NOT NULL,
    SCOPE_ID VARCHAR(36) NOT NULL,
    CREATE_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT PK_CLIENT_ALLOWED_SCOPE PRIMARY KEY (ID),
    CONSTRAINT FK_CLIENT_ALLOWED_SCOPE_CLIENT FOREIGN KEY (CLIENT_ID) REFERENCES CLIENT(ID),
    CONSTRAINT FK_CLIENT_ALLOWED_SCOPE_SCOPE FOREIGN KEY (SCOPE_ID) REFERENCES SCOPE(ID)                                  
);

-- The identities a client is allowed, (i.e. what user profile information they can access)
CREATE TABLE CLIENT_ALLOWED_IDENTITY (
    ID VARCHAR(36) NOT NULL DEFAULT (UUID()),
    CLIENT_ID VARCHAR(36) NOT NULL,
    IDENTITY INT NOT NULL,
    CREATE_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT PK_CLIENT_ALLOWED_IDENTITY PRIMARY KEY (ID),
    CONSTRAINT FK_CLIENT_ALLOWED_IDENTITY_CLIENT FOREIGN KEY (CLIENT_ID) REFERENCES CLIENT(ID)    
);

-- The grant types that a client can use (client creds and/or auth code)
CREATE TABLE CLIENT_ALLOWED_GRANT_TYPE (
    ID VARCHAR(36) NOT NULL DEFAULT (UUID()),
    CLIENT_ID VARCHAR(36) NOT NULL,
    GRANT_TYPE INT NOT NULL,
    CREATE_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT PK_CLIENT_ALLOWED_GRANT_TYPE PRIMARY KEY (ID),
    CONSTRAINT FK_CLIENT_ALLOWED_GRANT_TYPE_CLIENT FOREIGN KEY (CLIENT_ID) REFERENCES CLIENT(ID)
);

-- Defines a Resource / an API
CREATE TABLE RESOURCE(
    ID VARCHAR(36) NOT NULL DEFAULT (UUID()),
    NAME VARCHAR(250) NOT NULL,
    DESCRIPTION VARCHAR(1000) NULL,
    ACTIVE BIT NOT NULL DEFAULT 1,
    CREATE_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT PK_RESOURCE PRIMARY KEY (ID),
    CONSTRAINT UK_RESOURCE_NAME UNIQUE (NAME)
);

-- Scopes that the resource allows
CREATE TABLE RESOURCE_SCOPE (
    ID VARCHAR(36) NOT NULL DEFAULT (UUID()),
    RESOURCE_ID VARCHAR(36) NOT NULL,
    SCOPE_ID VARCHAR(36) NOT NULL,
    CREATE_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT PK_RESOURCE_SCOPE PRIMARY KEY (ID),
    CONSTRAINT FK_RESOURCE_SCOPE_RESOURCE FOREIGN KEY (RESOURCE_ID) REFERENCES RESOURCE(ID),
    CONSTRAINT FK_RESOURCE_SCOPE_SCOPE FOREIGN KEY  (SCOPE_ID) REFERENCES  SCOPE(ID)
);

