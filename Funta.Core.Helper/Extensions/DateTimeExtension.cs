using System;
using System.Collections.Generic;
using System.Text;

namespace Funta.Core.Helper.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToFullDateTime(this DateTime dateTime)
        {
            return DateTime.ParseExact(dateTime.ToString("MM/dd/yyyy H:m:s"), "MM/dd/yyyy H:m:s",
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
