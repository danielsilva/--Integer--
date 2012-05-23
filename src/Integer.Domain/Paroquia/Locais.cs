using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Integer.Domain.Paroquia
{
    public interface Locais
    {
        IEnumerable<Local> Todos();
        IEnumerable<Local> Todos(Expression<Func<Local, bool>> condicao);
        Local Com(Expression<Func<Local, bool>> condicao);

        void Salvar(Local local);
    }
}
