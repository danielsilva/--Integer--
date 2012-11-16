using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Acesso;
using Integer.Domain.Acesso.Exceptions;
using Integer.Domain.Paroquia;

namespace Integer.Api.Infra.Raven
{
    public static class UsuarioDocumentSessionExtensions
    {
        public static Usuario ObterUsuarioPorEmail(this IDocumentSession session, string email)
        {
            return session.Query<Usuario>().FirstOrDefault(u => u.Email == email);
        }

        public static bool ValidaUsuario(this IDocumentSession session, string email, string senha) 
        {
            var usuario = session.Query<Usuario>().FirstOrDefault(u => u.Email == email && u.Ativo);
            return (usuario != null && usuario.ValidaSenha(senha));
        }

        public static void CriarSenha(this IDocumentSession session, string usuarioId, string senha)         
        {
            var usuarioExistente = session.Query<Usuario>().FirstOrDefault(u => u.Id == usuarioId);
            if (usuarioExistente == null)
                throw new UsuarioInexistenteException();

            usuarioExistente.CriarSenha(senha);            
        }
    }
}