create table if not exists users(
    id serial PRIMARY KEY,
    username character varying(20) not null,
    password character varying(64) not null,
    token character varying(64),
    time character not null,
    cig_per_day integer not null,
    cig_count smallint not null,
    cig_price smallint not null
);