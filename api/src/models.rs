use serde::{Deserialize, Serialize};
use sqlx::prelude::FromRow;

#[derive(Debug, FromRow)]
pub struct User {
    pub id: i32,
    pub username: String,
    pub password: String,
    pub token: Option<String>
}

impl User {
    pub fn create<T: Into<String>>(username: T, password: T) -> Self {
        Self {
            id: 0,
            username: username.into(),
            password: password.into(),
            token: None
        }
    }
}

#[derive(Debug, FromRow, Serialize, PartialEq, Clone, Deserialize)]
pub struct UserData {
    pub user_id: i32,

    pub cig_per_day: i16,
    pub cig_count: i16,
    pub cig_price: i16,         // price of a pack of cigarettes

    pub currency: String,       // 3 chars. $, ₽, BYN...

    pub interval: i32,          // seconds
    pub last_input: i64,
}