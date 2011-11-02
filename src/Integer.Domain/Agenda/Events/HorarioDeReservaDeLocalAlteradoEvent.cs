using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;

namespace Integer.Domain.Agenda
{
    public class HorarioDeReservaDeLocalAlteradoEvent : DomainEvent
    {
        public HorarioDeReservaDeLocalAlteradoEvent(Evento evento, Reserva reserva)
        {
            this.Evento = evento;
            this.Reserva = reserva;
        }

        public Evento Evento { get; private set; }
        public Reserva Reserva { get; private set; }
    }
}
