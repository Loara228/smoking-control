
# Smoking control

## 💡 Idea

This project aims to reduce nicotine dependence.

## Why this project architecture?

I rarely use my phone and can forget about it for 6-8 hours. However, outdoors, you can't just pull out a laptop from your pocket to use the app.<br>
Therefore the application needs to be available on mobile device (Android) and PC (Windows). For this purpose, [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui) is employed to develop a cross-platform application for all necessary devices. Data is stored in [PostgreSQL](https://www.postgresql.org/), with [Actix Web](https://actix.rs/) handling communication between the client and server

## What does the application do?

The app displays the elapsed time since the last nicotine intake. It also shows a recommended interval that increases over time. Before attempting to break this interval and smoke again, the app will provide a warning to help prevent this. Additionally, the app allows users to view statistics and logs

**Important**: Users should take this seriously and manually open the app and log each smoking event, otherwise the application will not help you much

## Steps to run:

### PostgreSQL (Ubuntu)

```
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql.service
```
### PostgreSQL (Windows)

[installer](https://www.postgresql.org/download/windows/)

### Database

```sql
-- postgres=#
create user usr with password 'password';
CREATE DATABASE sc_db WITH OWNER = usr
grant all privileges on database sc_db to usr;
```

### Build

```bash
git clone https://github.com/Loara228/smoking-control.git
cd smoking-control
cargo run --release
```
