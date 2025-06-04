use sqlx::{postgres::PgQueryResult, Error, Pool, Postgres};

pub async fn create_table(pool: &Pool<Postgres>) -> Result<PgQueryResult, Error> {
    sqlx::query(include_str!("./queries/create_table.sql")).execute(pool).await
}