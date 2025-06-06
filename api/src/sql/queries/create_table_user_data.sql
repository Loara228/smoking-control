create table if not exists user_data (
    user_id integer not null,
    cig_per_day smallint not null,
    cig_count smallint not null,
    cig_price smallint not null,

    currency character varying(3) not null,

    interval integer not null,
    last_input bigint not null,
    
    unique(user_id)
);