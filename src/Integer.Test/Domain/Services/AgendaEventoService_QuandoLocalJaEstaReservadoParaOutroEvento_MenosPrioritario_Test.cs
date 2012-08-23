using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Services;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.DateAndTime;
using Integer.Infrastructure.Repository;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Services
{
    public class AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_MenosPrioritario_Test : InMemoryDataBaseTest
    {
        Evento eventoExistente, eventoNovo;

        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreReservas = TimeSpan.FromMinutes(59);

        Local localDesejado;
        DateTime dataInicioReservaExistente, dataFimReservaExistente;
        DateTime dataAtual;

        public AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_MenosPrioritario_Test() 
        {
            CriarNovoBancoDeDados();

            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;

            var eventos = new EventoRepository(DataBaseSession);
            agendaEventoService = new AgendaEventoService(eventos);
        }

        [Fact]
        public void QuandoAReservaNova_TerminaMenosDeUmaHoraAntes_SalvaEventoNovoComSucesso()
        {
            CriaReservaQueTerminaMenosDeUmaHoraAntes();

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        [Fact]
        public void QuandoAReservaNova_TerminaMenosDeUmaHoraAntes_EventoExistenteFicaComEstadoNaoAgendado()
        {
            CriaReservaQueTerminaMenosDeUmaHoraAntes();

            Assert.Equal(EstadoEventoEnum.NaoAgendado, eventoExistente.Estado);
        }

        [Fact]
        public void QuandoAReservaNova_TerminaMenosDeUmaHoraAntes_EventoExistenteFicaComConflitoReferenteAoEventoNovo()
        {
            CriaReservaQueTerminaMenosDeUmaHoraAntes();

            Assert.Equal(1, eventoExistente.Conflitos.Count());
            var conflito = eventoExistente.Conflitos.First();
            Assert.Equal(eventoNovo.Id, conflito.Evento.Id);
            Assert.Equal(MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade, conflito.Motivo);
        }

        public void CriaReservaQueTerminaMenosDeUmaHoraAntes() 
        {
            CriarEventoExistenteQueReservouLocal(TipoEventoEnum.Comum);

            eventoNovo = CriarEvento(TipoEventoEnum.GrandeMovimentoDePessoas, dataAtual, dataAtual.AddHours(4));
            var dataFimNovaReserva = dataInicioReservaExistente.AddMinutes(-59);
            var dataInicioNovaReserva = dataFimNovaReserva.AddHours(-2);
            eventoNovo.Reservar(localDesejado, dataInicioNovaReserva, dataFimNovaReserva);

            agendaEventoService.Agendar(eventoNovo);
            DataBaseSession.SaveChanges();
        }

        [Fact]
        public void QuandoAReservaNova_ComecaMenosDeUmaHoraDepois_SalvaEventoNovoComSucesso()
        {
            CriaReservaQueComecaMenosDeUmaHoraDepois();

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        [Fact]
        public void QuandoAReservaNova_ComecaMenosDeUmaHoraDepois_EventoExistenteFicaComEstadoNaoAgendado()
        {
            CriaReservaQueComecaMenosDeUmaHoraDepois();

            Assert.Equal(EstadoEventoEnum.NaoAgendado, eventoExistente.Estado);
        }

        [Fact]
        public void QuandoAReservaNova_ComecaMenosDeUmaHoraDepois_EventoExistenteFicaComConflitoReferenteAoEventoNovo()
        {
            CriaReservaQueComecaMenosDeUmaHoraDepois();

            Assert.Equal(1, eventoExistente.Conflitos.Count());
            var conflito = eventoExistente.Conflitos.First();
            Assert.Equal(eventoNovo.Id, conflito.Evento.Id);
            Assert.Equal(MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade, conflito.Motivo);
        }

        public void CriaReservaQueComecaMenosDeUmaHoraDepois()
        {
            CriarEventoExistenteQueReservouLocal(TipoEventoEnum.Comum);

            eventoNovo = CriarEvento(TipoEventoEnum.GrandeMovimentoDePessoas, dataAtual, dataAtual.AddHours(4));
            var dataInicioNovaReserva = dataFimReservaExistente.AddMinutes(59);
            var dataFimNovaReserva = dataInicioNovaReserva.AddHours(2);
            eventoNovo.Reservar(localDesejado, dataInicioNovaReserva, dataFimNovaReserva);

            agendaEventoService.Agendar(eventoNovo);
            DataBaseSession.SaveChanges();
        }

        [Fact]
        public void QuandoAReservaNova_SobrepoeReservaExistente_SalvaEventoNovoComSucesso()
        {
            CriaReservaQueSobrepoe();

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        [Fact]
        public void QuandoAReservaNova_SobrepoeReservaExistente_EventoExistenteFicaComEstadoNaoAgendado()
        {
            CriaReservaQueSobrepoe();

            Assert.Equal(EstadoEventoEnum.NaoAgendado, eventoExistente.Estado);
        }

        [Fact]
        public void QuandoAReservaNova_SobrepoeReservaExistente_EventoExistenteFicaComConflitoReferenteAoEventoNovo()
        {
            CriaReservaQueSobrepoe();

            Assert.Equal(1, eventoExistente.Conflitos.Count());
            var conflito = eventoExistente.Conflitos.First();
            Assert.Equal(eventoNovo.Id, conflito.Evento.Id);
            Assert.Equal(MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade, conflito.Motivo);
        }

        public void CriaReservaQueSobrepoe()
        {
            CriarEventoExistenteQueReservouLocal(TipoEventoEnum.Comum);

            eventoNovo = CriarEvento(TipoEventoEnum.GrandeMovimentoDePessoas, dataAtual, dataAtual.AddHours(4));
            var dataInicioNovaReserva = dataInicioReservaExistente.AddMinutes(-1);
            var dataFimNovaReserva = dataFimReservaExistente.AddMinutes(1);
            eventoNovo.Reservar(localDesejado, dataInicioNovaReserva, dataFimNovaReserva);

            agendaEventoService.Agendar(eventoNovo);
            DataBaseSession.SaveChanges();
        }

        private void CriarEventoExistenteQueReservouLocal(TipoEventoEnum tipoEvento) 
        {
            eventoExistente = CriarEvento(tipoEvento, dataAtual, dataAtual.AddHours(4));

            localDesejado = new Local("Um Local");
            dataInicioReservaExistente = dataAtual;
            dataFimReservaExistente = dataInicioReservaExistente.AddHours(1);
            eventoExistente.Reservar(localDesejado, dataInicioReservaExistente, dataFimReservaExistente);

            DataBaseSession.Store(eventoExistente);
            DataBaseSession.SaveChanges();
        }

        private Evento CriarEvento(TipoEventoEnum tipo, DateTime dataInicio, DateTime dataFim)
        {
            var grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }
    }
}
