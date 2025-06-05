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
        .execute(pool)
        .await
}

pub async fn get_user(id: i32, pool: &Pool<Postgres>) -> Result<Option<User>, Error> {
    let user: Option<User> = sqlx::query_as(&format!("select * from users where id = \'{id}\' limit 1;"))
        .fetch_optional(pool)
        .await?;

    Ok(user)
}

pub async fn try_auth(username: String, password: String, pool: &Pool<Postgres>) -> Result<String, Box<dyn std::error::Error>>  {
    let user: Option<User> = sqlx::query_as(&format!("select * from users where username = $1 and password = $2 limit 1;"))
        .bind(username)
        .bind(password)
        .fetch_optional(pool)
        .await?;

    match user {
        Some(user) => {
            const KEY: &str = "synTaverny";
            let timestamp = std::time::SystemTime::now().duration_since(std::time::SystemTime::UNIX_EPOCH).unwrap().as_secs();

            let hash = sha256::digest(format!("{timestamp}{}{KEY}", user.password));
            sqlx::query("update users set token = $1 where id = $2")
                .bind(hash.clone())
                .bind(user.id)
                .execute(pool)
                .await?;

            return Ok(hash);
        },
        None => {
            return Err("failed".into());
        },
    }
}