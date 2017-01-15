using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Worker.Helpers
{
    public static class JRDateTimeConvert
    {
        public static string ConvertToJRDateTimeFormat(DateTime datetime)
        {
            if (datetime==null)
            {
                datetime = DateTime.Now;
            }
            var date = datetime.Date;
            var newFormat = $"{date.Year}-{date.Month}-{date.Day}";
            return newFormat;
        }

    }
}
