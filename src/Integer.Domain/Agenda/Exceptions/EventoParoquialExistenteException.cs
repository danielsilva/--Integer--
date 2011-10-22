using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Agenda
{
    public class EventoParoquialExistenteException : Exception
    {
        private readonly IEnumerable<Evento> eventosParoquiais;

        public EventoParoquialExistenteException(IEnumerable<Evento> eventosParoquiais)
        {
            this.eventosParoquiais = eventosParoquiais;
        }

        public override string Message
        {
            get
            {
                StringBuilder msgErro = new StringBuilder();
                foreach (Evento eventoParoquial in eventosParoquiais)
                {                    
                    msgErro.AppendLine(String.Format("O evento paroquial '{0}' já está cadastrado para o horário: {1}.", eventoParoquial.Nome, eventoParoquial.Horario));
                }
                return msgErro.ToString();
            }
        }
    }
}
