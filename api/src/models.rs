pub struct User {
    id: i32,
    username: String,
    password: String,
    token: Option<String>,
    time: i64,
    cig_per_day: i16,
    cig_count: i16,
    cig_price: i16 // a pack of cigarettes
}