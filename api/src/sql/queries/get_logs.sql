select * from user_logs
where user_id = $1
order by id desc
limit $3 offset $2;