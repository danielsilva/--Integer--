using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client;
using Integer.Domain.Acesso;
using Integer.Domain.Acesso.Exceptions;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Criptografia;

namespace Integer.Api.Infra.Raven
{
    public static class UsuarioDocumentSessionExtensions
    {
        public static Usuario ObterUsuarioPorEmail(this IDocumentSession session, string email)
        {
            return session.Query<Usuario>().FirstOrDefault(u => u.Email == email);
        }

        public static Usuario ObterUsuario(this IDocumentSession session, string email, string senha) 
        {
            return session.Query<Usuario>().FirstOrDefault(u => u.Email == email && u.Senha == Encryptor.Encrypt(senha) && u.Ativo);
        }
        
        public static Usuario CriarUsuario(this IDocumentSession session, string nome, string email, string senha, string grupoId)
        {
            var usuario = new Usuario(nome, email, senha, grupoId);
            session.Store(usuario);

            return usuario;
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