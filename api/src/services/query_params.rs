use serde::Deserialize;

/// Used for user creation and authentication
#[derive(Deserialize)]
pub struct UserParams {
    pub username: String,
    pub password: String
}

#[derive(Deserialize)]
pub struct TokenParam {
    pub token: String
}

#[derive(Deserialize)]
pub struct IdParam {
    pub id: i32
}

#[derive(Deserialize)]
pub struct TimeParam {
    pub timestamp: i64 // unix, utc
}

#[derive(Deserialize)]
pub struct GetLogsParam {
    pub start: i32, // offset
    pub count: i32  // limit
}

#[derive(Deserialize)]
pub struct TimeZoneParam {
    pub timezone: i32
}