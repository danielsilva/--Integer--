using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Paroquia;
using Integer.Domain.Agenda;

namespace Integer.Web.Infra.Raven
{
    public static class TipoEventoDocumentSessionExtensions
    {
        public static IList<TipoEvento> ObterTiposDeEvento(this IDocumentSession session)
        {
            var tipos = new List<TipoEvento>();
            using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(15)))
            {
                tipos = session.Query<TipoEvento>().OrderBy(t => t.Nome).ToList();
            }
            return tipos;
        }
    }
}