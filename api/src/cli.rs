use clap::{Parser, Subcommand};

#[derive(Parser, Debug)]
#[command(about)]
pub struct Cli {
    #[arg(short, long, default_value = "127.0.0.1")]
    pub addr: String,

    #[command(subcommand)]
    pub command: RunCommand
}

#[derive(Subcommand, Debug)]
pub enum RunCommand {
    HTTP {
        #[arg(short, long, default_value = "8080")]
        port: i32
    },
    HTTPS {
        
    }
}



// #[arg(short, long, default_value = "8081")]
// pub port: String,