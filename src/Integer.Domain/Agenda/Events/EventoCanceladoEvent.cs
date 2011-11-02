using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;

namespace Integer.Domain.Agenda
{
    public class EventoCanceladoEvent : IDomainEvent
    {
        public EventoCanceladoEvent(Evento eventoCancelado)
        {
            this.Evento = eventoCancelado;
        }

        public Evento Evento { get; private set; }
    }
}
