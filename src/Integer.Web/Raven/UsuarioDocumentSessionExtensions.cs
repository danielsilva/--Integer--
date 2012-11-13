using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Acesso;
using Integer.Domain.Acesso.Exceptions;
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
            var usuario = session.Query<Usuario>().FirstOrDefault(u => u.Email == email && u.Ativo);
            return (usuario != null && usuario.ValidaSenha(senha));
        }

        public static void CriarUsuario(this IDocumentSession session, Usuario usuario, Grupo grupo)         
        {
            var usuarioExistente = session.Query<Usuario>().FirstOrDefault(u => u.Email == usuario.Email);
            if (usuarioExistente != null)
                throw new UsuarioExistenteException(usuarioExistente);

            grupo.PrecisaCriarUsuario = false;
            session.Store(usuario);
        }
    }
}