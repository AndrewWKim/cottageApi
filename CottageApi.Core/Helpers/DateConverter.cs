using System;

namespace CottageApi.Core.Helpers
{
    public static class DateConverter
    {
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string TwoDigitsDateValue(int value)
        {
            var valueString = value.ToString();

            if (valueString.Length > 1)
            {
                return valueString;
            }

            return $"0{value}";
        }
    }
}
