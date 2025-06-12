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

        public Int32 user_id;

        public Int16 cig_per_day;
        public Int16 cig_count;
        public Int16 cig_price;

        public string currency;

        public Int32 interval;
        public Int64 last_input;
    }
}
