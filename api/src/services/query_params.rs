use serde::Deserialize;

/// Used for user creation and authentication
#[derive(Deserialize)]
pub struct UserParams {
    pub username: String,
    pub password: String
}