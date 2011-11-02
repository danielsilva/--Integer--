using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using System.Linq.Expressions;
using Integer.Infrastructure.LINQExpressions;

namespace Integer.Domain.Services
{
    public class AgendaEventoService
    {
        private Eventos eventos;
        private readonly TimeSpan INTERVALO_MINIMO = TimeSpan.FromMinutes(59);

        public AgendaEventoService(Eventos eventos)
        {
            this.eventos = eventos;
        }

        public void Agendar(Evento novoEvento)
        {
            VerificaSeConflitaComEventoParoquial(novoEvento.DataInicio, novoEvento.DataFim);
            if (novoEvento.Tipo == TipoEventoEnum.Paroquial)
                DesmarcaEventosNaoParoquiaisQueExistiremNaMesmaData(novoEvento);
            
            VerificaDisponibilidadeDeLocais(novoEvento);

            eventos.Salvar(novoEvento);
        }

        private void VerificaSeConflitaComEventoParoquial(DateTime dataInicio, DateTime dataFim)
        {
            Func<Evento, bool> verificaConflitoDeHorario = CriarCondicaoParaVerificacaoDeConflitoDeHorario(dataInicio, dataFim);
            IEnumerable<Evento> eventosParoquiaisEmConflito = eventos.Todos(e => e.Tipo == TipoEventoEnum.Paroquial && verificaConflitoDeHorario(e));

            if (eventosParoquiaisEmConflito.Count() > 0)
                throw new EventoParoquialExistenteException(eventosParoquiaisEmConflito);
        }

        private void DesmarcaEventosNaoParoquiaisQueExistiremNaMesmaData(Evento novoEvento)
        {
            var verificaConflitoDeHorario = CriarCondicaoParaVerificacaoDeConflitoDeHorario(novoEvento.DataInicio, novoEvento.DataFim);

            var eventosNaoParoquiaisEmConflito = eventos.Todos(e => e.Tipo != TipoEventoEnum.Paroquial && verificaConflitoDeHorario(e));
            foreach (Evento eventoNaoParoquial in eventosNaoParoquiaisEmConflito)
            {
                eventoNaoParoquial.AdicionarConflito(novoEvento, MotivoConflitoEnum.ExisteEventoParoquialNaData);
            }
        }

        private Func<Evento, bool> CriarCondicaoParaVerificacaoDeConflitoDeHorario(DateTime dataInicio, DateTime dataFim)
        {
            DateTime dataInicioComIntervaloMinimo = dataInicio.Subtract(INTERVALO_MINIMO);
            DateTime dataFimComIntervaloMinimo = dataFim.Add(INTERVALO_MINIMO);

            Func<Evento, bool> dataInicioComIntervaloFicaDentroDoEvento = (e => e.DataInicio <= dataInicioComIntervaloMinimo && dataInicioComIntervaloMinimo <= e.DataFim);
            Func<Evento, bool> dataFimComIntervaloFicaDentroDoEvento = (e => e.DataInicio <= dataFimComIntervaloMinimo && dataFimComIntervaloMinimo <= e.DataFim);
            Func<Evento, bool> comecaAntesETerminaDepoisDoEvento = (e => dataInicioComIntervaloMinimo <= e.DataInicio && e.DataFim <= dataFimComIntervaloMinimo);

            return e => dataInicioComIntervaloFicaDentroDoEvento(e) || dataFimComIntervaloFicaDentroDoEvento(e) || comecaAntesETerminaDepoisDoEvento(e);
        }

        private void VerificaDisponibilidadeDeLocais(Evento novoEvento)
        {
            Func<Evento, bool> verificaQueReservouLocalNoMesmoHorario = CriarCondicaoParaVerificarReservaDeLocal(novoEvento);
            IEnumerable<Evento> eventosQueReservaramLocalNoMesmoHorario = eventos.Todos(e => verificaQueReservouLocalNoMesmoHorario(e));

            IList<Evento> eventosPrioritarios = new List<Evento>();
            foreach (Evento eventoQueReservouLocal in eventosQueReservaramLocalNoMesmoHorario)
            {
                if (eventoQueReservouLocal.PossuiPrioridadeSobre(novoEvento)) 
                    eventosPrioritarios.Add(eventoQueReservouLocal);
                else
                    eventoQueReservouLocal.AdicionarConflito(novoEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            }

            if (eventosPrioritarios.Count > 0)
                throw new LocalReservadoException(eventosPrioritarios);
        }

        private Func<Evento, bool> CriarCondicaoParaVerificarReservaDeLocal(Evento novoEvento) 
        {
            Expression<Func<Evento, bool>> predicate = PredicateBuilder.False<Evento>();

            foreach (Reserva reserva in novoEvento.Reservas)
            {
                var dataInicioReservada = reserva.DataInicio.Subtract(INTERVALO_MINIMO);
                var dataFimReservada = reserva.DataFim.AddMinutes(INTERVALO_MINIMO.Minutes);

                short idLocalReservado = reserva.Local.Id;
                predicate = predicate.Or(e => e.Reservas.Count(r => r.Local.Id == idLocalReservado
                                                                                    && ((r.DataInicio <= dataInicioReservada && dataInicioReservada <= r.DataFim)
                                                                                        || (r.DataInicio <= dataFimReservada && dataFimReservada <= r.DataFim)
                                                                                        || (dataInicioReservada <= r.DataInicio && r.DataFim <= dataFimReservada))) > 0);
            }

            return predicate.Compile();
        }
    }
}
