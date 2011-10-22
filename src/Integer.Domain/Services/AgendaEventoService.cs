using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;

namespace Integer.Domain.Services
{
    public class AgendaEventoService
    {
        private Eventos eventos;
        

        public AgendaEventoService(Eventos eventos)
        {
            this.eventos = eventos;
        }

        public void Agendar(Evento novoEvento)
        {
            VerificaSeConflitaComEventoParoquial(novoEvento.DataInicio, novoEvento.DataFim);
            if (novoEvento.Tipo == TipoEventoEnum.Paroquial)
                DesmarcaEventosNaoParoquiaisQueExistiremNaMesmaData(novoEvento);

            eventos.Salvar(novoEvento);
        }

        private void VerificaSeConflitaComEventoParoquial(DateTime dataInicio, DateTime dataFim)
        {
            var verificaConflitoDeHorario = CriarCondicaoParaVerificacaoDeConflitoDeHorario(dataInicio, dataFim);

            var eventosParoquiaisEmConflito = eventos.Todos(e => e.Tipo == TipoEventoEnum.Paroquial && verificaConflitoDeHorario(e));

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
            var intervaloMinimoEntreEventos = TimeSpan.FromMinutes(59);

            DateTime dataInicioComIntervaloMinimo = dataInicio.Subtract(intervaloMinimoEntreEventos);
            DateTime dataFimComIntervaloMinimo = dataFim.Add(intervaloMinimoEntreEventos);

            Func<Evento, bool> dataInicioComIntervaloFicaDentroDoEvento = (e => e.DataInicio <= dataInicioComIntervaloMinimo && dataInicioComIntervaloMinimo <= e.DataFim);
            Func<Evento, bool> dataFimComIntervaloFicaDentroDoEvento = (e => e.DataInicio <= dataFimComIntervaloMinimo && dataFimComIntervaloMinimo <= e.DataFim);
            Func<Evento, bool> comecaAntesETerminaDepoisDoEvento = (e => dataInicioComIntervaloMinimo <= e.DataInicio && e.DataFim <= dataFimComIntervaloMinimo);

            return e => dataInicioComIntervaloFicaDentroDoEvento(e) || dataFimComIntervaloFicaDentroDoEvento(e) || comecaAntesETerminaDepoisDoEvento(e);
        }
    }
}
