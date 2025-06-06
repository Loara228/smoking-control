use sqlx::{PgPool, Pool, Postgres};

use crate::{models::{User, UserData}, sql};

#[sqlx::test()]
async fn connection_test() {
    let _ = connect().await.unwrap();
}

#[sqlx::test()]
async fn create_tables_test() {
    let pool = connect().await.unwrap();

    sql::create_table(&pool).await.unwrap();
}

#[sqlx::test()]
async fn api_test() {
    let pool = connect().await.unwrap();
    sql::create_table(&pool).await.unwrap();

    let user = User::create(TEST_USERNAME, TEST_PASSWORD);
    let id = sql::users::insert_user(user, &pool).await.unwrap();
    let token = sql::try_auth(TEST_USERNAME.to_owned(), TEST_PASSWORD.to_owned(), &pool).await.unwrap();
    assert_eq!(id, sql::get_id(token, &pool).await.unwrap().unwrap());

    let data = sql::user_data::get_user_data(id, &pool).await.unwrap();
    
    assert!(data.is_none());
    
    let data = UserData {
        user_id: id,
        cig_per_day: 10_i16,
        cig_count: 20_i16,
        cig_price: 200_i16,
        currency: "₽".to_owned(),

        interval: 103600,
        last_input: 1000000000_i64
    };

    sql::user_data::update_user_data(id, data.clone(), &pool).await.unwrap();
    let data2 = sql::user_data::get_user_data(id, &pool).await.unwrap().unwrap();

    assert_eq!(data, data2);

    sql::users::delete_user(id, &pool).await.unwrap();
    sql::user_data::delete_user_data(id, &pool).await.unwrap();
}

async fn connect() -> Result<Pool<Postgres>, sqlx::Error> {
    PgPool::connect(&crate::db_url()).await
}

const TEST_USERNAME: &str = "hdfjghdlgkdfg";
const TEST_PASSWORD: &str = "sdjhflksdfhlksf";