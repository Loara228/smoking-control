-- Count of logs for today

SELECT 
    COUNT(*) AS log_count
FROM 
    user_logs
WHERE 
    user_id = $1
    AND to_timestamp(time) AT TIME ZONE $2 >= date_trunc('day', now() AT TIME ZONE $2)
    AND to_timestamp(time) AT TIME ZONE $2 < date_trunc('day', now() AT TIME ZONE $2) + interval '1 day';