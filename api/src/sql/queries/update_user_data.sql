update user_data
    set cig_per_day = $2,
    set cig_count = $3,
    set cig_price = $4,

    set currency = $5,

    set interval = $6,
    set last_input = $7;
where user_id = $1;