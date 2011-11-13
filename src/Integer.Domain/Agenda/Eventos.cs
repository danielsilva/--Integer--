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

        void Salvar(Evento evento);

        IEnumerable<Evento> ObterEventosAgendadosDoDia(DateTime dataReferencia, string idGrupo);
        IEnumerable<Evento> ObterTodosEventosDoDia(DateTime dataReferencia, string idGrupo);

        IEnumerable<Evento> ObterTodosEventosDaSemana(DateTime primeiroDiaDaSemana, DateTime ultimoDiaDaSemana, string idGrupo);
        IEnumerable<Evento> ObterEventosAgendadosDaSemana(DateTime primeiroDiaDaSemana, DateTime ultimoDiaDaSemana, string idGrupo);

        IEnumerable<Evento> ObterTodosEventosDoMes(DateTime dataReferencia, string idGrupo);
        IEnumerable<Evento> ObterEventosAgendadosDoMes(DateTime dataReferencia, string idGrupo);
    }
}
