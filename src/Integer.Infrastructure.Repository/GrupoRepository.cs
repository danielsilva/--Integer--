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
    public class GrupoRepository : Grupos
    {
        private dynamic documentSession;

        public GrupoRepository(dynamic documentSession)
        {
            this.documentSession = documentSession;
        }

        public IEnumerable<Grupo> Todos()
        {
            //return documentSession.Query<Grupo>();
            return null;
        }

        public IEnumerable<Grupo> Todos(Expression<Func<Grupo, bool>> condicao)
        {
            //return documentSession.Query<Grupo>().Where(condicao);
            return null;
        }

        public Grupo Com(Expression<Func<Grupo, bool>> condicao)
        {
            //return documentSession.Query<Grupo>().FirstOrDefault(condicao);
            return null;
        }

        public void Salvar(Grupo evento)
        {
            documentSession.Store(evento);
        }
    }
}
