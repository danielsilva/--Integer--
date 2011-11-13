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
    public class GrupoRepository : Grupos
    {
        private IDocumentSession documentSession;

        public GrupoRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public IEnumerable<Grupo> Todos()
        {
            return documentSession.Query<Grupo>();
        }

        public IEnumerable<Grupo> Todos(Expression<Func<Grupo, bool>> condicao)
        {
            return documentSession.Query<Grupo>().Where(condicao);
        }

        public Grupo Com(Expression<Func<Grupo, bool>> condicao)
        {
            return documentSession.Query<Grupo>().FirstOrDefault(condicao);
        }

        public void Salvar(Grupo evento)
        {
            documentSession.Store(evento);
        }
    }
}
