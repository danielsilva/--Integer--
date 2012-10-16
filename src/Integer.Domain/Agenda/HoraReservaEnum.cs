using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Integer.Infrastructure.Enums;

namespace Integer.Domain.Agenda
{
    public enum HoraReservaEnum
    {
        [Description("Manhã")]
        Manha = 1,

        [Description("Tarde")]
        Tarde = 2,

        [Description("Noite")]
        Noite = 3
    }

    public static class HoraReservaEnumExtensions 
    {
        public static string ToHoraReservaString(this IEnumerable<HoraReservaEnum> hora) 
        {
            return string.Join(" / ", hora.OrderBy(h => (int)h).Select(h => h.GetDescription()));
        }
    }
}
