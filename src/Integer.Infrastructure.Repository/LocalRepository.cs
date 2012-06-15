using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
//using Raven.Client;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;
using Integer.Domain.Paroquia;

namespace Integer.Infrastructure.Repository
{
    public class LocalRepository : Locais
    {
        private dynamic documentSession;

        public LocalRepository(dynamic documentSession)
        {
            this.documentSession = documentSession;
        }

        public IEnumerable<Local> Todos()
        {
            //return documentSession.Query<Local>();
            return null;
        }

        public IEnumerable<Local> Todos(Expression<Func<Local, bool>> condicao)
        {
            //return documentSession.Query<Local>().Where(condicao);
            return null;
        }

        public Local Com(Expression<Func<Local, bool>> condicao)
        {
            //return documentSession.Query<Local>().FirstOrDefault(condicao);
            return null;
        }

        public void Salvar(Local evento)
        {
            documentSession.Store(evento);
        }
    }
}
