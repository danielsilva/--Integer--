using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;

namespace Integer.Domain.Agenda
{
    public class ReservaDeLocalCanceladaEvent : DomainEvent
    {
        public ReservaDeLocalCanceladaEvent(Evento evento, IEnumerable<Reserva> reservas)
        {
            this.Evento = evento;
            this.Reservas = reservas;
        }

        public Evento Evento { get; private set; }
        public IEnumerable<Reserva> Reservas { get; private set; }
    }
}
