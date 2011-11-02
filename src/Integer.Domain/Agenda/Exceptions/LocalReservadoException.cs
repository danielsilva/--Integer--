using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    msgErro.AppendLine(String.Format("O evento '{0}' já reservou: " + Environment.NewLine, eventoPrioritario.Nome));
                    foreach (var reserva in eventoPrioritario.Reservas)
                    {
                        msgErro.AppendLine(String.Format("- '{0}' no horário: {1}", reserva.Local.Nome, reserva.Horario));
                    }
                    msgErro.AppendLine(Environment.NewLine);
                }
                return msgErro.ToString();
            }
        }
    }
}
