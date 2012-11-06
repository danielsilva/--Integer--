using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Rhino.Mocks;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Repository;
using Xunit;
using Integer.Domain.Agenda.Exceptions;

namespace Integer.UnitTests.Domain.Services
{
    public class AgendaEventoService_QuandoEventoNovoNaoParoquialConflitaComEventoParoquialExistente_Test : InMemoryDataBaseTest
    {
        AgendaEventoService agendaEventoService;
        Evento eventoParoquialExistente;

        public AgendaEventoService_QuandoEventoNovoNaoParoquialConflitaComEventoParoquialExistente_Test()
        {
            CriarNovoBancoDeDados();
            CriarEventoParoquialPreExistente();

            var eventos = new EventoRepository(DataBaseSession);
            agendaEventoService = new AgendaEventoService(eventos);
        }

        [Fact]
        public void QuandoEventoNovoNaoParoquial_AconteceMenosDeUmaHoraDepoisDoEventoParoquial_DisparaExcecao() 
        {
            DateTime dataInicioEvento = eventoParoquialExistente.DataFim.AddMinutes(59);
            DateTime dataFimEvento = dataInicioEvento.AddHours(1);
            var eventoNovoNaoParoquial = CriarEvento(TipoEventoEnum.Comum, dataInicioEvento, dataFimEvento);

            Assert.Throws<EventoParoquialExistenteException>(() => agendaEventoService.Agendar(eventoNovoNaoParoquial));
        }

        [Fact]
        public void QuandoEventoNovoNaoParoquial_AconteceUmaHoraDepoisDoEventoParoquial_CadastraComSucesso()
        {
            DateTime dataInicioEvento = eventoParoquialExistente.DataFim.AddMinutes(60);
            DateTime dataFimEvento = dataInicioEvento.AddHours(1);
            var eventoNovoNaoParoquial = CriarEvento(TipoEventoEnum.Comum, dataInicioEvento, dataFimEvento);

            agendaEventoService.Agendar(eventoNovoNaoParoquial);
            DataBaseSession.SaveChanges();

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        [Fact]
        public void QuandoEventoNovoNaoParoquial_AconteceMenosDeUmaHoraAntesDoEventoParoquial_DisparaExcecao()
        {
            DateTime dataFimEvento = eventoParoquialExistente.DataInicio.AddMinutes(-59);
            DateTime dataInicioEvento = dataFimEvento.AddHours(-1);
            var eventoNovoNaoParoquial = CriarEvento(TipoEventoEnum.Comum, dataInicioEvento, dataFimEvento);

            Assert.Throws<EventoParoquialExistenteException>(() => agendaEventoService.Agendar(eventoNovoNaoParoquial));
        }

        [Fact]
        public void QuandoEventoNovoNaoParoquial_AconteceUmaHoraAntesDoEventoParoquial_CadastraComSucesso()
        {
            DateTime dataFimEvento = eventoParoquialExistente.DataInicio.AddMinutes(-60);
            DateTime dataInicioEvento = dataFimEvento.AddHours(-1);
            var eventoNovoNaoParoquial = CriarEvento(TipoEventoEnum.Comum, dataInicioEvento, dataFimEvento);

            agendaEventoService.Agendar(eventoNovoNaoParoquial);
            DataBaseSession.SaveChanges();

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        [Fact]
        public void QuandoEventoNovoNaoParoquial_SobrepoeEventoParoquial_DisparaExcecao() 
        {
            DateTime dataInicioEvento = eventoParoquialExistente.DataInicio.AddMinutes(-1);
            DateTime dataFimEvento = eventoParoquialExistente.DataFim.AddMinutes(1);
            var novoEvento = CriarEvento(TipoEventoEnum.Comum, dataInicioEvento, dataFimEvento);

            Assert.Throws<EventoParoquialExistenteException>(() => agendaEventoService.Agendar(novoEvento));
        }

        private Evento CriarEvento(TipoEventoEnum tipo, DateTime dataInicio, DateTime dataFim)
        {
            Grupo grupo = MockRepository.GenerateStub<Grupo>();

            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }

        private void CriarEventoParoquialPreExistente()
        {
            DateTime dataInicio = DateTime.Now;
            DateTime dataFim = dataInicio.AddHours(4);
            eventoParoquialExistente = CriarEvento(TipoEventoEnum.Paroquial, dataInicio, dataFim);

            DataBaseSession.Store(eventoParoquialExistente);
            DataBaseSession.SaveChanges();
        }
    }
}
