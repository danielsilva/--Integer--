using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Raven.Client;

namespace Integer.Infrastructure.Repository
{
    public class EventoRepository : Eventos
    {
        private IDocumentSession documentSession;

        public EventoRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public IEnumerable<Evento> Todos(Func<Evento, bool> condicao)
        {
            return documentSession.Query<Evento>().Where(condicao);
        }

        public void Salvar(Evento evento)
        {
            documentSession.Store(evento);
            documentSession.SaveChanges();
        }
    }
}
