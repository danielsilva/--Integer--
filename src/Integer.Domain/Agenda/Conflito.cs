using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.DateAndTime;

namespace Integer.Domain.Agenda
{
    public class Conflito
    {
        protected Conflito() 
        {
        }

        public Conflito(Evento evento, MotivoConflitoEnum motivo)
        {
            this.Evento = evento;
            this.Motivo = motivo;
            this.Data = SystemTime.Now();
        }

        public virtual Evento Evento { get; private set; }
        public virtual DateTime Data { get; private set; }
        public MotivoConflitoEnum Motivo { get; private set; }
    }
}
