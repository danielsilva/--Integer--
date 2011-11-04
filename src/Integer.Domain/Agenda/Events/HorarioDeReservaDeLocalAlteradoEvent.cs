using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;

namespace Integer.Domain.Agenda
{
    public class HorarioDeReservaDeLocalAlteradoEvent : DomainEvent
    {
        public HorarioDeReservaDeLocalAlteradoEvent(Evento evento, IEnumerable<Reserva> reservasAlteradas)
        {
            this.Evento = evento;
            this.ReservasAlteradas = reservasAlteradas;
        }

        public Evento Evento { get; private set; }
        public IEnumerable<Reserva> ReservasAlteradas { get; private set; }
    }
}
