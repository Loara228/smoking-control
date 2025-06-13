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