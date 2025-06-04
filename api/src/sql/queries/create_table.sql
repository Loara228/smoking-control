create table if not exists users(
    id serial PRIMARY KEY,
    username character varying(20) not null,
    password character varying(64) not null,
    token character varying(64),
    time bigint not null,

    unique(username, token)
);