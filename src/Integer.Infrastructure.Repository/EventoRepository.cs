using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Raven.Client;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;

namespace Integer.Infrastructure.Repository
{
    public class EventoRepository : Eventos
    {
        private IDocumentSession documentSession;

        public EventoRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public IEnumerable<Evento> Todos(Expression<Func<Evento, bool>> condicao)
        {
            var filtro = condicao.And(e => e.Estado != EstadoEventoEnum.Cancelado)
                                 .Compile();

            return documentSession.Query<Evento>().Where(filtro);
        }

        public void Salvar(Evento evento)
        {
            documentSession.Store(evento);
            documentSession.SaveChanges();
        }


        public IEnumerable<Evento> QuePossuemConflitosCom(Evento evento)
        {
            return documentSession.Query<Evento>().Where(e => e.Conflitos.Any(c => c.Evento == evento));
        }

        public IEnumerable<Evento> QuePossuemConflitoCom(Evento evento, MotivoConflitoEnum motivo)
        {
            return documentSession.Query<Evento>().Where(e => e.Conflitos.Any(c => c.Evento == evento && c.Motivo == motivo));
        }
    }
}
