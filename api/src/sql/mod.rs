use sqlx::{postgres::PgQueryResult, Error, Pool, Postgres};
use crate::models::User;

pub async fn create_table(pool: &Pool<Postgres>) -> Result<PgQueryResult, Error> {
    sqlx::query(include_str!("./queries/create_table.sql")).execute(pool).await
}

pub async fn insert_user(user: User, pool: &Pool<Postgres>) -> Result<PgQueryResult, Error> {
    sqlx::query("insert into users (username, password, time) values ($1, $2, $3);")
        .bind(user.username)
        .bind(user.password)
        .bind(user.time)
        .execute(pool).await
}

pub async fn get_user(id: i32, pool: &Pool<Postgres>) -> Result<Option<User>, Error> {
    let user: Option<User> = sqlx::query_as(&format!("select * from users where id = \'{id}\' limit 4;"))
        .fetch_optional(pool).await?;

    Ok(user)
}