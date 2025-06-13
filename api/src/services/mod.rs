mod responses;
mod query_params;

use std::collections::HashSet;
use actix_web::{web::Query, *};
use crate::{models::{User, UserData}, services::query_params::{IdParam, TokenParam, UserParams}, sql, AppState};

#[get("/")]
async fn index() -> impl Responder {
    HttpResponse::Ok().body(
        r#":p"#
    )
}

/// # Returns
/// ```200``` Username<br>
/// ```404``` The user was not found<br>
/// ```500``` Error from sqlx
/// 
/// # Usage
/// http://127.0.0.1:8080/users/get/1
#[get("/users/get/{user_id}")]
async fn users_get(state: web::Data<AppState>, request: HttpRequest) -> impl Responder {
    match request.match_info().query("user_id").parse::<i32>() {
        Ok(id) => {
            match sql::users::get_user(id, &state.pool).await {
                Ok(user) => {
                    match user {
                        Some(user) => {
                            return HttpResponse::Ok().body(user.username);
                        },
                        None => {
                            // user not found
                            HttpResponse::NotFound().body("The user with this ID was not found!")
                        },
                    }
                },
                Err(_) => responses::internal_error(),
            }
        },
        Err(e) => responses::parse_failed(Box::new(e), "user_id")
    }
}

/// # Returns
/// ```200``` Success<br>
/// ```400``` Invalid format 
/// ```409``` If a user with that name already exists
/// 
/// # Usage
/// http://127.0.0.1:8080/users/create?username=my_username&password=my_password
#[get("users/create")]
async fn users_create(state: web::Data<AppState>, query: web::Query<UserParams>) -> HttpResponse {

    let username: &str = &query.username;
    let password: &str = &query.password;

    let mut invalid = false;
    
    let chars: HashSet<char> = HashSet::from(['!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '=', '[', '{', ']', '}', ';', ':', '<', '>', '|', '.', '/', '?', ',', '-']);
    
    if username.len() > 20 || username.len() < 2 {
        invalid = true;
    } else if password.len() > 50 || password.len() < 8 {
        invalid = true;
    }

    if !invalid {
        for c in username.chars() {
            if !(c.is_ascii_alphanumeric() || chars.contains(&c)) {
                invalid = true;
                break;
            }
        }
    }

    if !invalid {
        for c in password.chars() {
            if !(c.is_ascii_alphanumeric() || chars.contains(&c)) {
                invalid = true;
                break;
            }
        }
    }
    if invalid {
        return HttpResponse::BadRequest().body("failed");
    }

    let new_user = User::create(username,  &pwd_hash(password));
    match sql::users::insert_user(&new_user, &state.pool).await {
        Ok(_) => {
            return HttpResponse::Ok().body("success");
        },
        Err(_) => {
            return HttpResponse::Conflict().body("A user with that name already exists");
        },
    }
}

/// # Returns
/// ```200``` token<br> 
/// ```401``` Unauthorized (why? idk)
/// 
/// # Usage
/// http://127.0.0.1:8080/auth?username=my_username&password=my_password
#[get("auth")]
async fn auth(state: web::Data<AppState>, query: web::Query<UserParams>) -> impl Responder {
    match sql::try_auth(query.0.username.clone(), pwd_hash(&query.0.password), &state.pool).await {
        Ok(token) => {
            return HttpResponse::Ok().body(token);
        },
        Err(_) => {
            return responses::invalid_token();
        },
    }
}

/// # Returns
/// ```200``` UserData as JSON<br> 
/// ```401``` invalid token
/// ```404``` data not found
/// ```500``` Error from sqlx
/// 
/// # Usage
/// http://127.0.0.1:8080/users/data/get?token=token
#[get("/users/data/get")]
async fn get_user_data(state: web::Data<AppState>, query: web::Query<TokenParam>) -> impl Responder {
    match sql::get_id(query.token.clone(), &state.pool).await {
        Ok(id) => {
            match id {
                Some(id) => {
                    match sql::user_data::get_user_data(id, &state.pool).await {
                        Ok(data) => {
                            match data {
                                Some(data) => HttpResponse::Ok().json(data),
                                None => HttpResponse::NotFound().body("not found"),
                            }
                        },
                        Err(_) => responses::internal_error(),
                    }
                },
                None => responses::invalid_token(),
            }
        },
        Err(_) => responses::internal_error(),
    }
}

/// # Returns
/// ```200``` success<br> 
/// ```401``` invalid token
/// ```500``` Error from sqlx
/// 
/// # Usage
/// http://127.0.0.1:8080/users/data/set?token=token
#[get("users/data/set")]
async fn set_user_data(state: web::Data<AppState>, query_token: web::Query<TokenParam>, query_user: web::Query<UserData>) -> impl Responder {
    match sql::get_id(query_token.token.clone(), &state.pool).await {
        Ok(id) => {
            match id {
                Some(id) => {
                    match sql::user_data::update_user_data(id, &query_user.0, &state.pool).await {
                        Ok(_) => {
                            HttpResponse::Ok().body("success")
                        },
                        Err(_) => responses::internal_error(),
                    }
                },
                None => responses::invalid_token(),
            }
        },
        Err(_) => responses::internal_error(),
    }
}

/// # Returns
/// ```200``` success<br> 
/// ```401``` invalid token
/// ```500``` Error from sqlx
#[get("logs/add")]
async fn log_add(state: web::Data<AppState>, query_token: web::Query<TokenParam>) -> impl Responder {
    match sql::get_id(query_token.token.clone(), &state.pool).await {
        Ok(id_optiom) => {
            match id_optiom {
                Some(user_id) => {
                    match sql::logs::insert_log(user_id, &state.pool).await {
                        Ok(_) => HttpResponse::Ok().body("success"),
                        Err(_) => responses::internal_error(),
                    }
                },
                None => responses::invalid_token(),
            }
        },
        Err(_) => responses::internal_error(),
    }
}

pub(crate) fn pwd_hash(input: &str) -> String {
    sha256::digest(format!("{input}ksjdhfsklddfhskfhskdhfjsdfsd"))
}