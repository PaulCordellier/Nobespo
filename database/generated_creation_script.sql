﻿DROP TABLE IF EXISTS account;

CREATE TABLE account (
    account_id integer GENERATED BY DEFAULT AS IDENTITY,
    username character varying(25) NOT NULL,
    hashed_password bytea NOT NULL,
    CONSTRAINT "PK_account" PRIMARY KEY (account_id)
);

CREATE UNIQUE INDEX "IX_account_username" ON account (username);