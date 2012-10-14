using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Paroquia;

namespace Integer.Web.Infra.Raven
{
    public static class DocumentSessionExtensions
    {
        public static Grupo ObterGrupoPorEmail(this IDocumentSession session, string email)
        {
            return session.Query<Grupo>().FirstOrDefault(g => g.Email == email);
        }
    }
}