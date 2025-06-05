use serde::Deserialize;

#[derive(Deserialize)]
pub struct CreateUserParam {
    pub username: String,
    pub password: String
}