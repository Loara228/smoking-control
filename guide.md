
# Detailed guide for Linux (Ubuntu 24.04.2 LTS)

```<IP Adddress>``` - the static ip address of your server<br>
```<Domain.com>``` - Your domain

<!-- ```bash
ssh-keygen -t rsa
ssh-copy-id -i ./.ssh/id_rsa.pub root@<IP Address>
ssh root@<IP Address> -p 22
``` -->

# 🛠️ Steps to run

С/C++ compiler, rustc, cargo, open ssl, certbot

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

This directory contains your keys and certificates.

`privkey.pem`: the private key for your certificate.
`fullchain.pem`: the certificate file used in most server software.
`chain.pem`: used for OCSP stapling in Nginx >=1.3.7.
`cert.pem`: will break many server configurations, and should not be used reading further documentation (see link below).

WARNING: DO NOT MOVE OR RENAME THESE FILES! Certbot expects these files to remain in this location in order to function properly!

We recommend not moving these files. For more information, see the Certbot
User Guide at https://certbot.eff.org/docs/using.html#where-are-my-certificates.

### Set up the launch configuration

```bash
cd smoking-control/build
nano conf.sh
```

```txt
export SRV_IP="0.0.0.0"				# auto
export SRV_HOSTNAME="192.168.0.148"	# for connection from the app
export SRV_PORT="8081"              
export SRV_USE_SSL=false

export SRV_KEY="/etc/letsencrypt/live/<DOMAIN.ru>/key.pem"
export SRV_CERT="/etc/letsencrypt/live/<DOMAIN.ru>/cert.pem"
```

### Finally, run the server

```bash
chmod +x run_http.sh & ./run_http.sh
```