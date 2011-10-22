using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Agenda
{
    public interface Eventos
    {
        IEnumerable<Evento> Todos(Func<Evento, bool> condicao);

        void Salvar(Evento evento);
    }
}
