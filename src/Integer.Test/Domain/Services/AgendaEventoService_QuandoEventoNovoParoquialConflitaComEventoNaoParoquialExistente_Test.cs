using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Services;
using Integer.Infrastructure.Repository;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Services
{
    public class AgendaEventoService_QuandoEventoNovoParoquialConflitaComEventoNaoParoquialExistente_Test : InMemoryDataBaseTest
    {
        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreEventos = TimeSpan.FromMinutes(59);

        Evento eventoNaoParoquialExistente, eventoNovoParoquial;

        public AgendaEventoService_QuandoEventoNovoParoquialConflitaComEventoNaoParoquialExistente_Test() 
        {
            CriarNovoBancoDeDados();
            CriarEventoNaoParoquialPreExistente();

            var eventos = new EventoRepository(DataBaseSession);
            agendaEventoService = new AgendaEventoService(eventos);
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraDepois_EventoNaoParoquialExistenteFicaComEstadoNaoAgendado() 
        {
            CriarEventoNovoParoquialQueAconteceDepois(59);

            Assert.Equal(EstadoEventoEnum.NaoAgendado, eventoNaoParoquialExistente.Estado);
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraDepois_EventoNaoParoquialExistenteFicaComConflitoReferenteAoEventoParoquial()
        {
            CriarEventoNovoParoquialQueAconteceDepois(59);

            Assert.Equal(1, eventoNaoParoquialExistente.Conflitos.Count());
            var conflito = eventoNaoParoquialExistente.Conflitos.First();
            Assert.Equal(eventoNovoParoquial.Id, conflito.Evento.Id);
            Assert.Equal(MotivoConflitoEnum.ExisteEventoParoquialNaData, conflito.Motivo);
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraDepois_CadastraComSucesso()
        {
            CriarEventoNovoParoquialQueAconteceDepois(59);

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceUmaHoraDepois_EventoNaoParoquialExistentePermaneceAgendadoESemConflitos()
        {
            CriarEventoNovoParoquialQueAconteceDepois(60);

            Assert.Equal(EstadoEventoEnum.Agendado, eventoNaoParoquialExistente.Estado);
            Assert.Equal(0, eventoNaoParoquialExistente.Conflitos.Count());
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceUmaHoraDepois_CadastraComSucesso()
        {
            CriarEventoNovoParoquialQueAconteceDepois(60);

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        private void CriarEventoNovoParoquialQueAconteceDepois(int tempoEmMinutos) 
        {
            DateTime dataInicioEvento = eventoNaoParoquialExistente.DataFim.AddMinutes(tempoEmMinutos);
            DateTime dataFimEvento = dataInicioEvento.AddHours(1);
            eventoNovoParoquial = CriarEvento(TipoEventoEnum.Paroquial, dataInicioEvento, dataFimEvento);

            agendaEventoService.Agendar(eventoNovoParoquial);
            DataBaseSession.SaveChanges();
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraAntes_EventoNaoParoquialExistenteFicaComEstadoNaoAgendado()
        {
            CriarEventoNovoParoquialQueAconteceAntes(59);

            Assert.Equal(EstadoEventoEnum.NaoAgendado, eventoNaoParoquialExistente.Estado);
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraAntes_EventoNaoParoquialExistenteFicaComConflitoReferenteAoEventoParoquial()
        {
            CriarEventoNovoParoquialQueAconteceAntes(59);

            Assert.Equal(1, eventoNaoParoquialExistente.Conflitos.Count());
            var conflito = eventoNaoParoquialExistente.Conflitos.First();
            Assert.Equal(eventoNovoParoquial.Id, conflito.Evento.Id);
            Assert.Equal(MotivoConflitoEnum.ExisteEventoParoquialNaData, conflito.Motivo);
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceMenosDeUmaHoraAntes_CadastraComSucesso()
        {
            CriarEventoNovoParoquialQueAconteceAntes(59);

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceUmaHoraAntes_EventoNaoParoquialExistentePermaneceAgendadoESemConflitos()
        {
            CriarEventoNovoParoquialQueAconteceAntes(60);

            Assert.Equal(EstadoEventoEnum.Agendado, eventoNaoParoquialExistente.Estado);
            Assert.Equal(0, eventoNaoParoquialExistente.Conflitos.Count());
        }

        [Fact]
        public void QuandoEventoNovoParoquial_AconteceUmaHoraAntes_CadastraComSucesso()
        {
            CriarEventoNovoParoquialQueAconteceAntes(60);

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        private void CriarEventoNovoParoquialQueAconteceAntes(int tempoEmMinutos)
        {
            DateTime dataFimEvento = eventoNaoParoquialExistente.DataInicio.AddMinutes(-1 * tempoEmMinutos);
            DateTime dataInicioEvento = dataFimEvento.AddHours(-1);
            eventoNovoParoquial = CriarEvento(TipoEventoEnum.Paroquial, dataInicioEvento, dataFimEvento);

            agendaEventoService.Agendar(eventoNovoParoquial);
            DataBaseSession.SaveChanges();
        }

        [Fact]
        public void QuandoEventoNovoParoquial_Sobrepoe_EventoNaoParoquialExistenteFicaComEstadoNaoAgendado()
        {
            CriarEventoNovoParoquialQueSobrepoe();

            Assert.Equal(EstadoEventoEnum.NaoAgendado, eventoNaoParoquialExistente.Estado);
        }

        [Fact]
        public void QuandoEventoNovoParoquial_Sobrepoe_EventoNaoParoquialExistenteFicaComConflitoReferenteAoEventoParoquial()
        {
            CriarEventoNovoParoquialQueSobrepoe();

            Assert.Equal(1, eventoNaoParoquialExistente.Conflitos.Count());
            var conflito = eventoNaoParoquialExistente.Conflitos.First();
            Assert.Equal(eventoNovoParoquial.Id, conflito.Evento.Id);
            Assert.Equal(MotivoConflitoEnum.ExisteEventoParoquialNaData, conflito.Motivo);
        }

        [Fact]
        public void QuandoEventoNovoParoquial_Sobrepoe_CadastraComSucesso()
        {
            CriarEventoNovoParoquialQueSobrepoe();

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        private void CriarEventoNovoParoquialQueSobrepoe()
        {
            DateTime dataInicioEvento = eventoNaoParoquialExistente.DataInicio.AddMinutes(-1);
            DateTime dataFimEvento = eventoNaoParoquialExistente.DataFim.AddMinutes(1);
            eventoNovoParoquial = CriarEvento(TipoEventoEnum.Paroquial, dataInicioEvento, dataFimEvento);

            agendaEventoService.Agendar(eventoNovoParoquial);
            DataBaseSession.SaveChanges();
        }

        private Evento CriarEvento(TipoEventoEnum tipo, DateTime dataInicio, DateTime dataFim)
        {
            Grupo grupo = MockRepository.GenerateStub<Grupo>();

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
