use actix_web::{web, App, HttpServer};
use sqlx::{PgPool, Pool, Postgres};
use tracing::level_filters::LevelFilter;

mod sql;
mod models;
mod services;

#[derive(Clone)]
pub struct AppState {
    pool: Pool<Postgres>
}

impl AppState {
    pub async fn new() -> Self {
        
        Self {
            pool: PgPool::connect(&db_url()).await.unwrap()
        }
    }
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    tracing_subscriber::fmt().with_max_level(LevelFilter::INFO).init();
    let app_state = AppState::new().await;
    sql::create_table(&app_state.pool).await.unwrap();

    HttpServer::new(move || {
        App::new()
            .app_data(web::Data::new(app_state.clone()))
            .service(services::index)
        
    })
    .bind(("127.0.0.1", 8080))?
    .run()
    .await
}

fn db_url() -> String {
    let user: &str = "usr";
    let password: &str = "password";
    let db_name: &str = "sc_db";
    format!("postgres://{user}:{password}@localhost:5432/{db_name}")
}