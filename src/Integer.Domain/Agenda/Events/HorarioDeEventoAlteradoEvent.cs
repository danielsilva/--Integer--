using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;

namespace Integer.Domain.Agenda
{
    public class HorarioDeEventoAlteradoEvent : DomainEvent
    {
        public HorarioDeEventoAlteradoEvent(Evento eventoAlterado)
        {
            this.EventoAlterado = eventoAlterado;
        }

        public Evento EventoAlterado { get; private set; }
    }
}
