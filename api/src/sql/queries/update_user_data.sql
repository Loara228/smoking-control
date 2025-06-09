update user_data set

    cig_per_day = $2,
    cig_count = $3,
    cig_price = $4,

    currency = $5,

    interval = $6,
    last_input = $7

where user_id = $1;