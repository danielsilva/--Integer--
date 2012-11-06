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
    public class UsuarioRepository : Usuarios
    {
        private IDocumentSession documentSession;

        public UsuarioRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public Usuario Com(Expression<Func<Usuario, bool>> condicao)
        {
            return documentSession.Query<Usuario>().FirstOrDefault(condicao);
        }
    }
}
