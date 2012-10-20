using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Paroquia;
using Integer.Domain.Agenda;

namespace Integer.Web.Infra.Raven
{
    public static class EventoDocumentSessionExtensions
    {
        public static IEnumerable<Evento> ObterEventos(this IDocumentSession session, DateTime inicio, DateTime fim)
        {
            var eventos = new List<Evento>();
            //using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(1)))
            //{
                eventos = session.Query<Evento>().Where(e => e.Estado == EstadoEventoEnum.Agendado
                                                                    && (inicio <= e.DataInicio || inicio <= e.DataFim
                                                                        || fim >= e.DataInicio || fim >= e.DataFim)).ToList();
            //}
            return eventos;
        }
    }
}