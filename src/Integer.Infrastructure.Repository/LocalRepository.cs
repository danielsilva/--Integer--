using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Raven.Client;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;
using Integer.Domain.Paroquia;

namespace Integer.Infrastructure.Repository
{
    public class LocalRepository : Locais
    {
        private IDocumentSession documentSession;

        public LocalRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public IEnumerable<Local> Todos()
        {
            return documentSession.Query<Local>();
        }

        public IEnumerable<Local> Todos(Expression<Func<Local, bool>> condicao)
        {
            return documentSession.Query<Local>().Where(condicao);
        }

        public Local Com(Expression<Func<Local, bool>> condicao)
        {
            return documentSession.Query<Local>().FirstOrDefault(condicao);
        }

        public void Salvar(Local evento)
        {
            documentSession.Store(evento);
        }
    }
}
