using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;

namespace Integer.Domain.Agenda
{
    public class ReservaDeLocalAlteradaEvent : DomainEvent
    {
        public ReservaDeLocalAlteradaEvent(Evento evento)
        {
            this.Evento = evento;
        }

        public Evento Evento { get; private set; }
    }
}
