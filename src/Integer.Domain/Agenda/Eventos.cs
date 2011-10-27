using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Integer.Domain.Agenda
{
    public interface Eventos
    {
        IEnumerable<Evento> Todos(Expression<Func<Evento, bool>> condicao);

        void Salvar(Evento evento);
    }
}
