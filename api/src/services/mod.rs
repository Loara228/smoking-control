use actix_web::*;

#[get("/")]
async fn index() -> impl Responder {
    HttpResponse::Ok().body("index")
}