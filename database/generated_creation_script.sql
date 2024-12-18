﻿DROP TABLE IF EXISTS account, film_comment, serie_comment CASCADE;

CREATE TABLE account (
    account_id integer GENERATED BY DEFAULT AS IDENTITY,
    username character varying(25) NOT NULL,
    hashed_password bytea NOT NULL,
    CONSTRAINT pk_account PRIMARY KEY (account_id)
);

CREATE TABLE film_comment (
    film_comment_id integer GENERATED BY DEFAULT AS IDENTITY,
    comment_text character varying(20000) NOT NULL,
    publish_date_and_time date NOT NULL DEFAULT (CURRENT_DATE),
    tmdb_film_id integer NOT NULL,
    user_id integer NOT NULL,
    CONSTRAINT pk_film_comment PRIMARY KEY (film_comment_id),
    CONSTRAINT fk_user_film_comment FOREIGN KEY (user_id) REFERENCES account (account_id) ON DELETE CASCADE
);

CREATE TABLE serie_comment (
    serie_comment_id integer GENERATED BY DEFAULT AS IDENTITY,
    comment_text character varying(20000) NOT NULL,
    publish_date_and_time date NOT NULL DEFAULT (CURRENT_DATE),
    tmdb_serie_id integer NOT NULL,
    user_id integer NOT NULL,
    CONSTRAINT pk_serie_comment PRIMARY KEY (serie_comment_id),
    CONSTRAINT fk_user_serie_comment FOREIGN KEY (user_id) REFERENCES account (account_id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX ix_account_username ON account (username);

CREATE INDEX ix_film_comment_tmdb_film_id ON film_comment (tmdb_film_id);

CREATE INDEX ix_film_comment_user_id ON film_comment (user_id);

CREATE INDEX ix_serie_comment_tmdb_serie_id ON serie_comment (tmdb_serie_id);

CREATE INDEX ix_serie_comment_user_id ON serie_comment (user_id);

CREATE VIEW all_comments AS
  SELECT user_id, comment_text, 'film'::varchar(10) AS commented_object, tmdb_film_id FROM film_comment
    UNION
  SELECT user_id, comment_text, 'serie'::varchar(10) AS commented_object, tmdb_serie_id FROM serie_comment;

-- Connect view to ef core :
-- https://learn.microsoft.com/en-us/ef/core/modeling/keyless-entity-types?tabs=data-annotations
