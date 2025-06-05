mod responses;
mod query_params;

use actix_web::*;
use crate::{models::User, services::query_params::UserParams, sql, AppState};

#[get("/")]
async fn index() -> impl Responder {
    HttpResponse::Ok().body(
        r#":p"#
    )
}

/// # Returns
/// ```200``` todo: User as JSON<br>
/// ```404``` The user was not found<br>
/// ```500``` Unknown error from sqlx/postgreSQL
/// 
/// # Usage
/// http://127.0.0.1:8080/users/get/1
#[get("/users/get/{user_id}")]
async fn users_get(state: web::Data<AppState>, request: HttpRequest) -> impl Responder {
    match request.match_info().query("user_id").parse::<i32>() {
        Ok(id) => {
            match sql::get_user(id, &state.pool).await {
                Ok(user) => {
                    match user {
                        Some(user) => {
                            // user found
                            HttpResponse::Ok().body(user.username)
                        },
                        None => {
                            // user not found
                            HttpResponse::NotFound().body("The user with this ID was not found!")
                        },
                    }
                },
                Err(e) => {
                    // exception from database
                    trace_error(Box::new(e));
                    responses::internal_error()
                },
            }
        },
        Err(e) => responses::parse_failed(Box::new(e), "user_id")
    }
}

/// # Returns
/// ```200``` Success<br>
/// ```409``` If a user with that name already exists
/// 
/// # Usage
/// http://127.0.0.1:8080/users/create?username=my_username&password=my_password
#[get("users/create")]
async fn users_create(state: web::Data<AppState>, query: web::Query<UserParams>) -> HttpResponse {
    let new_user = User::create(query.username.clone(), query.password.clone());
    match sql::insert_user(new_user, &state.pool).await {
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
/// ```401``` failed
/// 
/// # Usage
/// http://127.0.0.1:8080/auth?username=my_username&password=my_password
#[get("auth")]
async fn auth(state: web::Data<AppState>, query: web::Query<UserParams>) -> impl Responder {
    match sql::try_auth(query.0.username.clone(), query.0.username, &state.pool).await {
        Ok(token) => {
            return HttpResponse::Ok().body(token);
        },
        Err(e) => {
            return HttpResponse::Unauthorized().body(format!("{e}"));
        },
    }
}

// // http://127.0.0.1:8080/users/create/username/password
// #[get("/users/create/{username}/{password}")]
// async fn user_create(state: web::Data<AppState>, request: HttpRequest) -> HttpResponse {
//     match request.match_info().query("username").parse::<String>() {
//         Ok(username) => {
//             match request.match_info().query("password").parse::<String>() {
//                 Ok(password) => {
//                     let new_user = User::create(username, password);
//                     match sql::insert_user(new_user, &state.pool).await {
//                         Ok(_) => {
//                             return HttpResponse::Ok().body("success");
//                         },
//                         Err(sqlx) => {
//                             trace_error(Box::new(sqlx));
//                             return responses::internal_error();
//                         },
//                     }
//                 },
//                 Err(e) => {
//                     return responses::parse_failed(Box::new(e), "password")
//                 },
//             }
//         },
//         Err(e) => {
//             return responses::parse_failed(Box::new(e), "username")
//         },
//     }
// }

fn trace_error(e: Box<dyn std::error::Error>) {
    tracing::error!("{e:?}");
}