use actix_web::{web, App, HttpServer};
use sqlx::{PgPool, Pool, Postgres};
use tracing::level_filters::LevelFilter;

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
    let app_state = AppState::new().await;
    sql::create_table(&app_state.pool).await.unwrap();

    HttpServer::new(move || {
        App::new()
            .app_data(web::Data::new(app_state.clone()))
            .service(services::index)
            .service(services::auth)
            
            .service(services::users_get)
            .service(services::users_create)

            .service(services::get_user_data)
            .service(services::set_user_data)
        
    })
    .bind(("0.0.0.0", 8080))?
    // .bind(("127.0.0.1", 8080))?
    .run()
    .await
}

pub fn db_url() -> String {
    let user: &str = "usr";
    let password: &str = "password";
    let db_name: &str = "sc_db";
    format!("postgres://{user}:{password}@localhost:5432/{db_name}")
}

//  天上太陽紅呀紅彤彤哎
//  心中的太陽是毛澤東啊
//  他領導我們得解放啊
//  人民翻身當家做主人
//  依呀依子哟喂 呀而呀子哟啊
//  人民翻身當家做主人

//  天上太陽紅呀紅彤彤哎
//  心中的太陽是毛澤東啊
//  他領導我們奮勇前進啊
//  革命江山一呀片紅
//  依呀依子哟喂 呀而呀子哟啊
//  革命江山一呀片紅

//  天上太陽紅呀紅彤彤哎
//  心中的太陽是毛澤東啊
//  他領導我們奮勇前進啊
//  革命江山一呀一片紅
//  嗖啦啦子 嗖啦啦子
//  一呀一片紅哎