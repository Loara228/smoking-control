using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smoking_control.Models
{
    public class UserData
    {
        public UserData()
        {
            currency = string.Empty;
        }

        public override string ToString()
        {
            return $"user_id: {user_id}," +
                $"cig_per_day: {cig_per_day}," +
                $"cig_count: {cig_count}," +
                $"cig_price: {cig_price}," +
                $"currency: {currency}," +
                $"interval: {interval}," +
                $"last_input: {last_input}";
        }

        public Int32 user_id;

        public Int16 cig_per_day;
        public Int16 cig_count;
        public Int16 cig_price;

        public string currency;

        public Int32 interval;
        public Int64 last_input;
    }
}
