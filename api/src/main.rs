use std::net::SocketAddrV4;

use actix_web::{web, App, HttpServer};
use clap::Parser;
use sqlx::{PgPool, Pool, Postgres};
use tracing::level_filters::LevelFilter;

use crate::cli::Cli;

mod cli;
mod sql;
mod models;
mod services;
#[cfg(test)]
mod tests;

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

    let args = Cli::parse();

    let app_state = AppState::new().await;
    sql::create_table(&app_state.pool).await.unwrap();

    let server = HttpServer::new(move || {
        App::new()
            .app_data(web::Data::new(app_state.clone()))

            // .service(actix_files::Files::new("/", "src/static"))

            .service(services::auth)
            .service(services::verify_token)
            
            .service(services::users_get)
            .service(services::users_create)
            .service(services::users_inc_interval)

            .service(services::get_user_data)
            .service(services::set_user_data)

            .service(services::log_add)
            .service(services::log_delete)
            .service(services::get_logs)
            .service(services::get_logs_today)
        
    });

    match &args.command {
        cli::RunCommand::HTTP { port } => {
            let addr: SocketAddrV4 = format!("{}:{}", args.addr, *port).parse().unwrap();

            server.bind(addr)?
                .run()
                .await
        },
        cli::RunCommand::HTTPS {} => todo!()
    }
}

// unix utc time
pub fn cur_time() -> Result<i64, std::time::SystemTimeError> {
    Ok(std::time::SystemTime::now()
            .duration_since(std::time::UNIX_EPOCH)?
            .as_secs() as i64)
}

pub fn db_url() -> String {
    let user: &str = "usr";
    let password: &str = "password";
    let db_name: &str = "sc_db";
    format!("postgres://{user}:{password}@localhost:5432/{db_name}")
}