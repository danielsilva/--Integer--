using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Infrastructure.DateAndTime
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }

}
