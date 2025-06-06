use actix_web::*;
use std::error::Error;
pub fn parse_failed(error: Box<dyn Error>, param_name: &str) -> HttpResponse {
    HttpResponse::BadRequest().body(format!(
        "Failed to parse parameter with name \'{param_name}\'.\nException: {error}."
    ))
}

pub fn internal_error() -> HttpResponse {
    HttpResponse::InternalServerError()
        .body("An unexpected error occurred. Please try again later.")
}

pub fn invalid_token() -> HttpResponse {
    HttpResponse::Unauthorized().body("invalid token")
}