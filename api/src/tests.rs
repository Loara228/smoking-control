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
    delete_test_user(&pool).await;

    let user = User::create(TEST_USERNAME, &crate::services::pwd_hash(TEST_PASSWORD));
    let id = sql::users::insert_user(&user, &pool).await.unwrap();
    println!("Создан тестовый пользователь c ID: {id}");
    let token = sql::try_auth(TEST_USERNAME.to_owned(), crate::services::pwd_hash(TEST_PASSWORD), &pool).await.unwrap();
    println!("token: {token}");

    assert_eq!(id, sql::get_id(token, &pool).await.unwrap().unwrap());
    println!("Проверили get_id");

    let data = sql::user_data::get_user_data(id, &pool).await.unwrap();
    println!("Проверили get_user_data");
    
    assert!(data.is_none());
    println!("get_user_data вернул None");
    
    let data = UserData {
        user_id: id,
        cig_per_day: 10_i16,
        cig_count: 20_i16,
        cig_price: 200_i16,
        currency: "₽".to_owned(),

        interval: 103600,
        last_input: 1000000000_i64
    };

    sql::user_data::update_user_data(id, &data, &pool).await.unwrap();
    println!("Данные пользователя созданы");
    sql::user_data::update_user_data(id, &data, &pool).await.unwrap();
    println!("Данные пользователя обновлены");

    let data2 = sql::user_data::get_user_data(id, &pool).await.unwrap().unwrap();
    println!("Данные пользователя получены");

    assert_eq!(data, data2);
    println!("Данные не повредились");

    sql::users::delete_user(id, &pool).await.unwrap();
    sql::user_data::delete_user_data(id, &pool).await.unwrap();
    println!("Пользователь и его данные удалены");
}

async fn delete_test_user(pool: &Pool<Postgres>) {
    // На случай если прошлый тест не дошел до конца.
    println!("Проверяем существует ли тестовый пользователь");
    let id: Option<i32> = sqlx::query_scalar(&format!("select id from users where username = '{}';", TEST_USERNAME))
            .fetch_optional(pool)
            .await
            .unwrap();
    if id.is_some() {
        println!("Уничтожаем тестового пользователя");
        sqlx::query(&format!("delete from users where id = {};", id.unwrap()))
            .execute(pool)
            .await
            .unwrap();
        _ = sqlx::query(&format!("delete from user_data where id = {};", id.unwrap()))
            .execute(pool)
            .await;
    } else {
        println!("Пользователь пока не существует, но скоро будет")
    }
}
async fn connect() -> Result<Pool<Postgres>, sqlx::Error> {
    PgPool::connect(&crate::db_url()).await
}

const TEST_USERNAME: &str = "hdfjghdlgkdfg";
const TEST_PASSWORD: &str = "sdjhflksdfhlksf";