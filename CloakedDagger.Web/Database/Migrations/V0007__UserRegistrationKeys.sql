CREATE TABLE USER_REGISTRATION_KEY (
    ID UUID NOT NULL DEFAULT (uuid_generate_v4()),
    KEY_VAL VARCHAR(100) NOT NULL UNIQUE,
    USES_REMAINING INT NOT NULL,
    ACTIVE BOOLEAN NOT NULL DEFAULT TRUE,
    CREATE_DATE TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT PK_USER_REGISTRATION_KEY PRIMARY KEY (ID)
);

CREATE TRIGGER USER_REGISTRATION_KEY_UPDATE_MODIFIED_DATE BEFORE UPDATE
    ON USER_REGISTRATION_KEY FOR EACH ROW EXECUTE PROCEDURE
    UPDATE_MODIFIED_DATE_TIMESTAMP();

CREATE TABLE USER_REGISTRATION_KEY_USE (
    ID UUID NOT NULL DEFAULT (uuid_generate_v4()),
    USER_REGISTRATION_KEY_ID UUID NOT NULL,
    USER_ID UUID NOT NULL,
    CREATE_DATE TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    MODIFIED_DATE TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT PK_URKU PRIMARY KEY (ID),
    CONSTRAINT FK_URKU_USER FOREIGN KEY (USER_ID) REFERENCES "user"(ID),
    CONSTRAINT FK_URKU_URK FOREIGN KEY (USER_REGISTRATION_KEY_ID) REFERENCES USER_REGISTRATION_KEY(ID),
    CONSTRAINT UK_USER_URK UNIQUE (USER_REGISTRATION_KEY_ID, USER_ID)    
);

CREATE TRIGGER USER_REGISTRATION_KEY_USE_UPDATE_MODIFIED_DATE BEFORE UPDATE
    ON USER_REGISTRATION_KEY_USE FOR EACH ROW EXECUTE PROCEDURE
    UPDATE_MODIFIED_DATE_TIMESTAMP();
