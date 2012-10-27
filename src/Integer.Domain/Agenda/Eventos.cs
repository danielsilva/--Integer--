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
        IEnumerable<Evento> QuePossuemConflitosCom(Evento evento);
        IEnumerable<Evento> QuePossuemConflitoCom(Evento evento, MotivoConflitoEnum motivoConflitoEnum);
        IEnumerable<Evento> QueReservaramOMesmoLocal(Evento evento);

        void Salvar(Evento evento);
    }
}
