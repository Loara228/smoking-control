create table if not exists user_logs (
    id serial primary key,
    user_id int not null,
    time bigint not null,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);