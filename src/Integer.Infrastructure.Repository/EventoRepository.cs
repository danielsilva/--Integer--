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
            var ev = documentSession.Query<Evento>()
                .Where(e => e.Estado != EstadoEventoEnum.Cancelado)
                .Where(condicao).ToList();

            return ev;
        }

        public void Salvar(Evento evento)
        {
            documentSession.Store(evento);
        }
        
        public IEnumerable<Evento> QuePossuemConflitosCom(Evento evento)
        {
            return documentSession.Query<Evento>().Where(e => e.Conflitos.Any(c => c.Evento.Id.Equals(evento.Id)));
        }

        public IEnumerable<Evento> QuePossuemConflitoCom(Evento evento, MotivoConflitoEnum motivo)
        {
            return documentSession.Query<Evento>().Where(e => e.Conflitos.Any(c => c.Evento.Id.Equals(evento.Id) && c.Motivo == motivo));
        }

        public IEnumerable<Evento> QueReservaramOMesmoLocal(Evento evento)
        {
            var query = documentSession.Advanced.LuceneQuery<Evento>("ReservasMap");

            query = query.OpenSubclause();
            for (int i = 0; i < evento.Reservas.Count(); i++)
            {
                var reserva = evento.Reservas.ToList()[i];
                query = query
                    .Not.WhereEquals("Id", evento.Id)
                    .WhereEquals("IdLocal", reserva.Local.Id)
                    .AndAlso().WhereEquals("Data", reserva.Data)
                    .AndAlso().OpenSubclause();

                for (int j = 0; j < reserva.Hora.Count; j++)
                {
                    query = query.WhereEquals("Hora", (int)reserva.Hora[j]);
                    if (j + 1 < reserva.Hora.Count)
                        query = query.OrElse();
                }
                query = query.CloseSubclause().CloseSubclause();

                if (i + 1 < evento.Reservas.Count())
                    query = query.OrElse().OpenSubclause();
            }

            return query.ToList();
        }
    }
}
