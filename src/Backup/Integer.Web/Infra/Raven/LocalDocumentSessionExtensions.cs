using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Paroquia;

namespace Integer.Web.Infra.Raven
{
    public static class LocalDocumentSessionExtensions
    {
        public static IList<Local> ObterLocais(this IDocumentSession session)
        {
            var locais = new List<Local>();
            using (session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(15)))
            {
                locais = session.Query<Local>().OrderBy(l => l.Nome).ToList();
            }
            return locais;
        }
    }
}