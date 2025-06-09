create table if not exists users(
    id serial PRIMARY KEY,
    username character varying(20) not null,
    password character(64) not null,
    token character(64),

    unique(username)
);