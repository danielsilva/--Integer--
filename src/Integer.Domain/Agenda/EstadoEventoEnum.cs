using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Integer.Domain.Agenda
{
    public enum EstadoEventoEnum
    {
        [Description("Agendado")]
        Agendado = 1,
        [Description("Não agendado")]
        NaoAgendado = 2,
        [Description("Cancelado")]
        Cancelado = 3
    }
}
