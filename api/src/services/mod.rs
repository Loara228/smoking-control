use actix_web::*;

use crate::{sql, AppState};

#[get("/")]
async fn index() -> impl Responder {
    HttpResponse::Ok().body("index")
}

#[get("/users/{user_id}")]
async fn user(state: web::Data<AppState>, request: HttpRequest) -> impl Responder {
    match request.match_info().query("user_id").parse::<i32>() {
        Ok(id) => {
            match sql::get_user(id, &state.pool).await.unwrap() {
                Some(user) => HttpResponse::Ok().body(user.username),
                None => HttpResponse::NotFound().body("The user with this ID was not found!"),
            }
            
        },
        Err(_) => HttpResponse::BadRequest().body("user_id")
    }
}