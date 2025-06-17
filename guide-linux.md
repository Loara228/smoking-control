
# Detailed guide for Linux (Ubuntu 24.04.2 LTS)

```<IP Adddress>``` - the static ip address of your server<br>
```<Domain.com>``` - Your domain

# 🛠️ Steps to run

## Connect

```bash
ssh-keygen -t rsa
ssh-copy-id -i ./.ssh/id_rsa.pub root@<IP Address>
ssh root@<IP Address> -p 22
```

## С/C++ compiler, rustc, cargo, open ssl, certbot

```bash
sudo apt update
sudp apt upgrade
sudo apt install gcc
curl --proto '=https' --tlsv1.2 -sSf https://sh.rustup.rs | sh
sudo apt-get install pkg-config libssl-dev
sudo apt install certbot
```

### PostgreSQL

```bash
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql.service
sudo -i -u postgres
psql
```

```sql
create user usr with password 'password';
CREATE DATABASE sc_db WITH OWNER = usr;
grant all privileges on database sc_db to usr;
-- exit
postgres=# \q
```

### Clone

```
https://github.com/Loara228/smoking-control.git
```

### Certbot

```bash
nslookup <DOMAIN.ru>
sudo certbot certonly --standalone -d <DOMAIN.ru>
```

WARNING: DO NOT MOVE OR RENAME THESE FILES! Certbot expects these files to remain in this location in order to function properly!

### Set up the launch configuration

```bash
cd smoking-control/build
nano conf.sh
```

```txt
export SRV_IP="0.0.0.0"
export SRV_HOSTNAME="<Domain.com>" # This is for the app. It is not used to configure the server.
export SRV_PORT="8081"
export SRV_USE_SSL=true

export SRV_KEY="/etc/letsencrypt/live/<Domain.com>/privkey.pem"
export SRV_CERT="/etc/letsencrypt/live/<Domain.com>/fullchain.pem"
```

### Finally, run the server

```bash
chmod +x run_https.sh & ./run_http.sh
```