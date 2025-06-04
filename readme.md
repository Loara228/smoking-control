
### Installation of PostgreSQL:

### Linux

```
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql.service
```

### Windows

Download [installer](https://www.postgresql.org/download/windows/) and run it

### Creating the database

```sql
-- postgres=#
create user usr with password 'password';
CREATE DATABASE sc_db WITH OWNER = usr
grant all privileges on database sc_db to usr;
alter database db1 owner to usr;
```
