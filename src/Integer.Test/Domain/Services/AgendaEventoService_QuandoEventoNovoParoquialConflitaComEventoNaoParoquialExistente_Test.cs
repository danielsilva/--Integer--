using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Services;
using Integer.Infrastructure.Repository;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;

namespace Integer.UnitTests.Domain.Services
{
    [TestFixture]
    public class AgendaEventoService_QuandoEventoNovoParoquialConflitaComEventoNaoParoquialExistente_Test : InMemoryDataBaseTest
    {
        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreEventos = TimeSpan.FromMinutes(59);

        Evento eventoNaoParoquialExistente, eventoNovoParoquial;

        [SetUp]
        public void Init() 
        {
            CriarNovoBancoDeDados();
            CriarEventoNaoParoquialPreExistente();

            var eventos = new EventoRepository(DataBaseSession);
            agendaEventoService = new AgendaEventoService(eventos);
        }

        [Test]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraDepois_EventoNaoParoquialExistenteFicaComEstadoNaoAgendado() 
        {
            CriarEventoNovoParoquialQueAconteceMenosDeUmaHoraDepois();

            Assert.AreEqual(EstadoEventoEnum.NaoAgendado, eventoNaoParoquialExistente.Estado);
        }

        [Test]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraDepois_EventoNaoParoquialExistenteFicaComConflitoReferenteAoEventoParoquial()
        {
            CriarEventoNovoParoquialQueAconteceMenosDeUmaHoraDepois();

            Assert.AreEqual(1, eventoNaoParoquialExistente.Conflitos.Count());
            var conflito = eventoNaoParoquialExistente.Conflitos.First();
            Assert.AreEqual(eventoNovoParoquial, conflito.Evento);
        }

        private void CriarEventoNovoParoquialQueAconteceMenosDeUmaHoraDepois() 
        {
            DateTime dataInicioEvento = eventoNaoParoquialExistente.DataFim.AddMinutes(59);
            DateTime dataFimEvento = dataInicioEvento.AddHours(1);
            eventoNovoParoquial = CriarEvento(TipoEventoEnum.Paroquial, dataInicioEvento, dataFimEvento);

            agendaEventoService.Agendar(eventoNovoParoquial);
        }

        [Test]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraAntes_EventoNaoParoquialExistenteFicaComEstadoNaoAgendado()
        {
            CriarEventoNovoParoquialQueAconteceMenosDeUmaHoraAntes();

            Assert.AreEqual(EstadoEventoEnum.NaoAgendado, eventoNaoParoquialExistente.Estado);
        }

        [Test]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraAntes_EventoNaoParoquialExistenteFicaComConflitoReferenteAoEventoParoquial()
        {
            CriarEventoNovoParoquialQueAconteceMenosDeUmaHoraAntes();

            Assert.AreEqual(1, eventoNaoParoquialExistente.Conflitos.Count());
            var conflito = eventoNaoParoquialExistente.Conflitos.First();
            Assert.AreEqual(eventoNovoParoquial, conflito.Evento);
        }

        private void CriarEventoNovoParoquialQueAconteceMenosDeUmaHoraAntes()
        {
            DateTime dataFimEvento = eventoNaoParoquialExistente.DataInicio.AddMinutes(-59);
            DateTime dataInicioEvento = dataFimEvento.AddHours(-1);
            eventoNovoParoquial = CriarEvento(TipoEventoEnum.Paroquial, dataInicioEvento, dataFimEvento);

            agendaEventoService.Agendar(eventoNovoParoquial);
        }

        [Test]
        public void QuandoEventoNovoParoquial_Sobrepoe_EventoNaoParoquialExistenteFicaComEstadoNaoAgendado()
        {
            CriarEventoNovoParoquialQueSobrepoe();

            Assert.AreEqual(EstadoEventoEnum.NaoAgendado, eventoNaoParoquialExistente.Estado);
        }

        [Test]
        public void QuandoEventoNovoParoquial_Sobrepoe_EventoNaoParoquialExistenteFicaComConflitoReferenteAoEventoParoquial()
        {
            CriarEventoNovoParoquialQueSobrepoe();

            Assert.AreEqual(1, eventoNaoParoquialExistente.Conflitos.Count());
            var conflito = eventoNaoParoquialExistente.Conflitos.First();
            Assert.AreEqual(eventoNovoParoquial, conflito.Evento);
        }

        private void CriarEventoNovoParoquialQueSobrepoe()
        {
            DateTime dataInicioEvento = eventoNaoParoquialExistente.DataInicio.AddMinutes(1);
            DateTime dataFimEvento = eventoNaoParoquialExistente.DataFim.AddMinutes(-1);
            eventoNovoParoquial = CriarEvento(TipoEventoEnum.Paroquial, dataInicioEvento, dataFimEvento);

            agendaEventoService.Agendar(eventoNovoParoquial);
        }

        private Evento CriarEvento(TipoEventoEnum tipo, DateTime dataInicio, DateTime dataFim)
        {
            Grupo grupo = new Grupo("Grupo");

            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }

        private void CriarEventoNaoParoquialPreExistente()
        {
            DateTime dataInicio = DateTime.Now;
            DateTime dataFim = dataInicio.AddHours(4);
            eventoNaoParoquialExistente = CriarEvento(TipoEventoEnum.Comum, dataInicio, dataFim);

            DataBaseSession.Store(eventoNaoParoquialExistente);
            DataBaseSession.SaveChanges();
        }
    }
}
