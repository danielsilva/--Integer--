using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Raven.Client;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;
using Integer.Domain.Acesso;

namespace Integer.Infrastructure.Repository
{
    public class UsuarioTokenRepository : UsuarioTokens
    {
        private IDocumentSession documentSession;

        public UsuarioTokenRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public UsuarioToken Com(Expression<Func<UsuarioToken, bool>> condicao)
        {
            return documentSession.Query<UsuarioToken>().FirstOrDefault(condicao);
        }

        public void Salvar(UsuarioToken usuarioToken) 
        {
            documentSession.Store(usuarioToken);
        }
    }
}
