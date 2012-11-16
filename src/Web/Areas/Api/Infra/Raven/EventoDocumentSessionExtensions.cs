using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Agenda;

namespace Integer.Api.Infra.Raven
{
    public static class EventoDocumentSessionExtensions
    {
        public static IEnumerable<Evento> ObterEventosAgendados(this IDocumentSession session, DateTime inicio, DateTime fim)
        {
            var eventos = new List<Evento>();
            using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(2)))
            {
                eventos = session.Query<Evento>().Where(e => e.Estado == EstadoEventoEnum.Agendado
                                                                    && ((inicio <= e.DataInicio && e.DataInicio <= fim)
                                                                        || (inicio <= e.DataFim && e.DataFim <= fim))).ToList();
            }
            return eventos;
        }

        public static IEnumerable<Evento> ObterEventos(this IDocumentSession session, DateTime inicio, DateTime fim)
        {
            var eventos = new List<Evento>();
            eventos = session.Query<Evento>().Where(e => e.Estado != EstadoEventoEnum.Cancelado
                                                            && ((inicio <= e.DataInicio && e.DataInicio <= fim)
                                                                || (inicio <= e.DataFim && e.DataFim <= fim))).ToList();
            return eventos;
        }
    }
}