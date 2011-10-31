using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Agenda
{
    public class LocalReservadoException : Exception
    {
        private readonly Evento evento;
        private readonly IEnumerable<Reserva> reservasQuePossuemConflito;

        public LocalReservadoException(Evento evento, IEnumerable<Reserva> reservasQuePossuemConflito)
        {
            this.evento = evento;
            this.reservasQuePossuemConflito = reservasQuePossuemConflito;
        }

        public override string Message
        {
            get
            {
                StringBuilder msgErro = new StringBuilder();
                foreach (Evento eventoPrioritario in eventos)
                {
                    foreach (var reserva in eventoPrioritario.ReservasDeLocais)
                    {
                        msgErro.AppendLine(String.Format("O local '{0}' já está reservado para o evento '{1}' no horário: {2}", reserva.Local.Nome, eventoPrioritario.Nome, reserva.Horario));
                    }
                }
                return msgErro.ToString();
            }
        }
    }
}
