using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Paroquia;

namespace Integer.Api.Infra.Raven
{
    public static class GrupoDocumentSessionExtensions
    {
        public static Grupo ObterGrupoPorEmail(this IDocumentSession session, string email)
        {
            return session.Query<Grupo>().FirstOrDefault(g => g.Email == email);
        }

        public static Grupo ObterGrupoPorId(this IDocumentSession session, string id)
        {
            return session.Query<Grupo>().FirstOrDefault(g => g.Id == id);
        }
    }
}