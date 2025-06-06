use sqlx::{Error, Pool, Postgres};
use crate::models::User;

pub async fn create_table(pool: &Pool<Postgres>) -> Result<(), Error> {
    sqlx::query(include_str!("./queries/create_table_users.sql")).execute(pool).await?;
    sqlx::query(include_str!("./queries/create_table_user_data.sql")).execute(pool).await?;

    Ok(())
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

pub async fn get_id(token: String, pool: &Pool<Postgres>) -> Result<Option<i32>, Error> {
    let id: Option<i32> = sqlx::query_scalar("select id from users where token = $1")
        .bind(token)
        .fetch_optional(pool)
        .await?;

    Ok(id)
}

pub mod users {
    use crate::models::*;
    use sqlx::{Error, Pool, Postgres};

    pub async fn insert_user(user: User, pool: &Pool<Postgres>) -> Result<i32, Error> {
        let a: i32 = sqlx::query_scalar("insert into users (username, password) values ($1, $2) returning id;")
            .bind(user.username)
            .bind(user.password)
            .fetch_one(pool)
            .await
            .unwrap();
    
        Ok(a)
    }
    
    pub async fn delete_user(id: i32, pool: &Pool<Postgres>) -> Result<(), Error> {
        sqlx::query("delete from users where id = $1;")
            .bind(id)
            .execute(pool)
            .await?;
    
        Ok(())
    }
    
    pub async fn get_user(id: i32, pool: &Pool<Postgres>) -> Result<Option<User>, Error> {
        let user: Option<User> = sqlx::query_as(&format!("select * from users where id = $1 limit 1;"))
            .bind(id)
            .fetch_optional(pool)
            .await?;
    
        Ok(user)
    }
}

pub mod user_data {
    use crate::models::*;
    use sqlx::{Error, Pool, Postgres};

    pub async fn get_user_data(user_id: i32, pool: &Pool<Postgres>) -> Result<Option<UserData>, Error> {
        let data: Option<UserData> = sqlx::query_as("select * from user_data where user_id = $1 limit 1;")
            .bind(user_id)
            .fetch_optional(pool)
            .await?;

        Ok(data)
    }

    pub async fn delete_user_data(id: i32, pool: &Pool<Postgres>) -> Result<(), Error> {
        sqlx::query("delete from user_data where user_id = $1;")
            .bind(id)
            .execute(pool)
            .await?;

        Ok(())
    }

    pub async fn update_user_data(user_id: i32, data: UserData, pool: &Pool<Postgres>) -> Result<(), Error> {
        let id: Option<i32> = sqlx::query_scalar("select user_id from user_data where user_id = $1 limit 1;")
            .bind(user_id)
            .fetch_optional(pool)
            .await?;

        match id {
            Some(id) => {
                sqlx::query(include_str!("./queries/update_user_data.sql"))
                    .bind(data.user_id)
                    .bind(data.cig_per_day)
                    .bind(data.cig_count)
                    .bind(data.cig_price)
                    .bind(data.currency)
                    .bind(data.interval)
                    .bind(data.last_input)
                    .execute(pool)
                    .await?;
            },
            None => {
                sqlx::query(include_str!("./queries/insert_user_data.sql"))
                    .bind(data.user_id)
                    .bind(data.cig_per_day)
                    .bind(data.cig_count)
                    .bind(data.cig_price)
                    .bind(data.currency)
                    .bind(data.interval)
                    .bind(data.last_input)
                    .execute(pool)
                    .await?;
            },
        }

        Ok(())
    }
}