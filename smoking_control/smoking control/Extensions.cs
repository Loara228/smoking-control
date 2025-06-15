using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smoking_control
{
    public static class Extensions
    {
        public static string AsFormattedString(this TimeSpan t, bool ignoreSeconds = false)
        {
            char sn(int v)
            {
                if (v > 1)
                    return 's';
                return ' ';

            }

            string d = t.Days > 0 ? $" {t.Days} day{sn(t.Days)}" : "";
            string h = t.Hours > 0 ? $" {t.Hours} hour{sn(t.Hours)}" : "";
            string m = t.Minutes > 0 ? $" {t.Minutes} minute{sn(t.Minutes)}" : "";
            string s = ignoreSeconds ? "" : (t.Seconds > 0 ? $" {t.Seconds} second{sn(t.Seconds)}" : "");
            var result = $"{d}{h}{m}{s}";
            return result.Length > 0 ? result.Substring(1, result.Length - 1) : "-";
        }
    }
}
