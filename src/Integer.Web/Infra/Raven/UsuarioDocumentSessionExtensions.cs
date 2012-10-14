using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Paroquia;

namespace Integer.Web.Infra.Raven
{
    public static class UsuarioDocumentSessionExtensions
    {
        public static Usuario ObterUsuarioPorEmail(this IDocumentSession session, string email)
        {
            return session.Query<Usuario>().FirstOrDefault(u => u.Email == email);
        }

        public static bool ValidaUsuario(this IDocumentSession session, string email, string senha) 
        {
            var usuario = session.Query<Usuario>().FirstOrDefault(u => u.Email == email);
            return (usuario != null && usuario.ValidaSenha(senha));
        }
    }
}