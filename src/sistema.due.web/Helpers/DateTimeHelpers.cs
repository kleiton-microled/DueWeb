using System;

namespace Sistema.DUE.Web.Helpers
{
    public static class DateTimeHelpers
    {
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(timestamp) / 1000d)).ToLocalTime();
        }
    }
}