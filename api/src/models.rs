use sqlx::prelude::FromRow;

#[derive(Debug, FromRow)]
pub struct User {
    pub id: i32,
    pub username: String,
    pub password: String,
    pub token: Option<String>,
    pub time: i64
}

pub struct UserData {
    pub id: i32,
    pub cig_per_day: i16,
    pub cig_count: i16,
    pub cig_price: i16 // a pack of cigarettes
}