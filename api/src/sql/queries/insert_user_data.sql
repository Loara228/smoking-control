insert into user_data (
    user_id, 

    cig_per_day,
    cig_count,
    cig_price,

    currency,

    interval,
    last_input
) 
values (
    $1, $2, $3, $4, $5, $6, $7
);