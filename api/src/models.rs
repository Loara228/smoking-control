use sqlx::prelude::FromRow;

#[derive(Debug, FromRow)]
pub struct User {
    pub id: i32,
    pub username: String,
    pub password: String,
    pub token: Option<String>,
    pub time: i64
}

impl User {
    pub fn create(username: String, password: String) -> Self {
        Self {
            id: 0,
            username,
            password,
            token: None,
            time: 0
        }
    }
}

pub struct UserData {
    pub id: i32,
    pub cig_per_day: i16,
    pub cig_count: i16,
    pub cig_price: i16 // a pack of cigarettes
}