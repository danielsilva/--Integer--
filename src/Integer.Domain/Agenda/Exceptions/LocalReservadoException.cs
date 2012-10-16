using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Enums;

namespace Integer.Domain.Agenda
{
    public class LocalReservadoException : Exception
    {
        private readonly IEnumerable<Evento> eventos;

        public LocalReservadoException(IEnumerable<Evento> eventos)
        {
            this.eventos = eventos;
        }

        public override string Message
        {
            get
            {
                StringBuilder msgErro = new StringBuilder();
                foreach (Evento eventoPrioritario in eventos)
                {
                    msgErro.AppendLine(String.Format("O evento '{0}' já reservou: ", eventoPrioritario.Nome));
                    foreach (var reserva in eventoPrioritario.Reservas)
                    {
                        msgErro.AppendLine(String.Format("- '{0}' no horário: {1} ({2})", reserva.Local.Nome, reserva.Data.ToString("dd/MM/yyyy"), reserva.Hora.ToHoraReservaString()));
                    }
                }
                return msgErro.ToString();
            }
        }
    }
}
