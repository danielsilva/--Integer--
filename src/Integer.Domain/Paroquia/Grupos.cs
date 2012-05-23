using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Integer.Domain.Paroquia
{
    public interface Grupos
    {
        IEnumerable<Grupo> Todos();
        IEnumerable<Grupo> Todos(Expression<Func<Grupo, bool>> condicao);
        Grupo Com(Expression<Func<Grupo, bool>> condicao);

        void Salvar(Grupo grupo);
    }
}
