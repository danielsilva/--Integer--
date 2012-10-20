using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Raven.Client;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;

namespace Integer.Infrastructure.Repository
{
    public class EventoRepository : Eventos
    {
        private IDocumentSession documentSession;

        public EventoRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public IEnumerable<Evento> Todos(Expression<Func<Evento, bool>> condicao)
        {
            var filtro = condicao.And(e => e.Estado != EstadoEventoEnum.Cancelado);

            var ev = documentSession.Query<Evento>().Where(filtro).ToList();

            return ev;
        }

        public void Salvar(Evento evento)
        {
            documentSession.Store(evento);
        }
        
        public IEnumerable<Evento> QuePossuemConflitosCom(Evento evento)
        {
            return documentSession.Query<Evento>().Where(e => e.Conflitos.Any(c => c.Evento.Equals(evento)));
        }

        public IEnumerable<Evento> QuePossuemConflitoCom(Evento evento, MotivoConflitoEnum motivo)
        {
            return documentSession.Query<Evento>().Where(e => e.Conflitos.Any(c => c.Evento.Equals(evento) && c.Motivo == motivo));
        }

        public IEnumerable<Evento> ObterTodosEventosDoMes(DateTime data, string idGrupo)
        {
            Expression<Func<Evento, bool>> predicate = PredicateBuilder.InitializeWithTrue<Evento>();
            if (!String.IsNullOrEmpty(idGrupo))
            {
                predicate = predicate.And(e => e.Grupo.Id == idGrupo);
            }
            return documentSession.Query<Evento>().Where(predicate.And(e => e.Estado != EstadoEventoEnum.Cancelado
                                                                            && (e.DataInicio.Month == data.Month || e.DataFim.Month == data.Month))).ToList();
        }

        public IEnumerable<Evento> ObterTodosEventosDaSemana(DateTime dataInicio, DateTime dataFim, string idGrupo)
        {
            Expression<Func<Evento, bool>> predicate = PredicateBuilder.InitializeWithTrue<Evento>();
            if (!String.IsNullOrEmpty(idGrupo))
            {
                predicate = predicate.And(e => e.Grupo.Id == idGrupo);
            }
            return documentSession.Query<Evento>().Where(predicate.And(e => e.Estado != EstadoEventoEnum.Cancelado
                                                                            && (dataInicio < e.DataInicio || dataInicio < e.DataFim
                                                                                || e.DataInicio < dataFim || e.DataFim < dataFim)));
        }

        public IEnumerable<Evento> ObterTodosEventosDoDia(DateTime data, string idGrupo)
        {
            Expression<Func<Evento, bool>> predicate = PredicateBuilder.InitializeWithTrue<Evento>();
            if (!String.IsNullOrEmpty(idGrupo))
            {
                predicate = predicate.And(e => e.Grupo.Id == idGrupo);
            }
            return documentSession.Query<Evento>().Where(predicate.And(e => e.Estado != EstadoEventoEnum.Cancelado
                                                                        && (e.DataInicio.Day == data.Day || e.DataFim.Day == data.Day)));
        }

        public IEnumerable<Evento> ObterEventosAgendadosDoMes(DateTime data, string idGrupo)
        {
            return ObterTodosEventosDoMes(data, idGrupo).Where(e => e.Estado == EstadoEventoEnum.Agendado);
        }

        public IEnumerable<Evento> ObterEventosAgendadosDaSemana(DateTime dataInicio, DateTime dataFim, string idGrupo)
        {
            return ObterTodosEventosDaSemana(dataInicio, dataFim, idGrupo).Where(e => e.Estado == EstadoEventoEnum.Agendado);
        }

        public IEnumerable<Evento> ObterEventosAgendadosDoDia(DateTime data, string idGrupo)
        {
            return ObterTodosEventosDoDia(data, idGrupo).Where(e => e.Estado == EstadoEventoEnum.Agendado);
        }
    }
}
