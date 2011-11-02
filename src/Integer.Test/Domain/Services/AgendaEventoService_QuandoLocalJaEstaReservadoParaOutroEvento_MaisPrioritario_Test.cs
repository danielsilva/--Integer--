using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Services;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.DateAndTime;
using Integer.Infrastructure.Repository;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Services
{
    [TestFixture]
    public class AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_MaisPrioritario_Test : InMemoryDataBaseTest
    {
        Evento eventoExistente, eventoNovo;

        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreReservas = TimeSpan.FromMinutes(59);

        Local localDesejado;
        DateTime dataInicioReservaExistente, dataFimReservaExistente;
        DateTime dataAtual;

        [SetUp]
        public void init() 
        {
            CriarNovoBancoDeDados();

            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;

            var eventos = new EventoRepository(DataBaseSession);
            agendaEventoService = new AgendaEventoService(eventos);
        }

        [Test]
        [ExpectedException(typeof(LocalReservadoException))]
        public void QuandoAReservaNova_TerminaMenosDeUmaHoraAntes_DisparaExcecao() 
        {
            CriarEventoExistenteQueReservouLocal(TipoEventoEnum.GrandeMovimentoDePessoas);

            eventoNovo = CriarEvento(TipoEventoEnum.Comum, dataAtual, dataAtual.AddHours(4));
            var dataFimNovaReserva = dataInicioReservaExistente.AddMinutes(-59);
            var dataInicioNovaReserva = dataFimNovaReserva.AddHours(-2);
            eventoNovo.Reservar(localDesejado, dataInicioNovaReserva, dataFimNovaReserva);

            agendaEventoService.Agendar(eventoNovo);
        }

        [Test]
        [ExpectedException(typeof(LocalReservadoException))]
        public void QuandoAReservaNova_ComecaMenosDeUmaHoraDepois_DisparaExcecao()
        {
            CriarEventoExistenteQueReservouLocal(TipoEventoEnum.GrandeMovimentoDePessoas);

            Evento outroEvento = CriarEvento(TipoEventoEnum.Comum, dataAtual, dataAtual.AddHours(4));
            var dataInicioNovaReserva = dataFimReservaExistente.AddMinutes(59);
            var dataFimNovaReserva = dataInicioNovaReserva.AddHours(2);
            outroEvento.Reservar(localDesejado, dataInicioNovaReserva, dataFimNovaReserva);

            agendaEventoService.Agendar(outroEvento);
        }

        [Test]
        [ExpectedException(typeof(LocalReservadoException))]
        public void QuandoAReservaNova_SobrepoeReservaExistente_DisparaExcecao()
        {
            CriarEventoExistenteQueReservouLocal(TipoEventoEnum.GrandeMovimentoDePessoas);

            Evento outroEvento = CriarEvento(TipoEventoEnum.Comum, dataAtual, dataAtual.AddHours(4));
            var dataInicioNovaReserva = dataInicioReservaExistente.AddMinutes(-1);
            var dataFimNovaReserva = dataFimReservaExistente.AddMinutes(1);
            outroEvento.Reservar(localDesejado, dataInicioNovaReserva, dataFimNovaReserva);

            agendaEventoService.Agendar(outroEvento);
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
            var grupo = new Grupo("Grupo");
            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }
    }
}
