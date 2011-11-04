using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.DateAndTime;
using Integer.Infrastructure.DocumentModelling;

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

        public DenormalizedReference<Evento> Evento { get; private set; }
        public DateTime Data { get; private set; }
        public MotivoConflitoEnum Motivo { get; private set; }
    }
}
