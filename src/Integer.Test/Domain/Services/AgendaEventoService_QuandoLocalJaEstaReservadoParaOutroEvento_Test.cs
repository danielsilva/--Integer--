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
    public class AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_Test : InMemoryDataBaseTest
    {
        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreReservas = TimeSpan.FromMinutes(59);

        Local localReservado;
        DateTime dataInicioReservaExistente, dataFimReservaExistente;
        DateTime dataAtual;

        [SetUp]
        public void init() 
        {
            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;

            CriarEventoExistenteQueReservouLocal();

            var eventos = new EventoRepository(DataBaseSession);
            agendaEventoService = new AgendaEventoService(eventos);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        [Ignore]
        public void QuandoAReservaDeLocal_ComecaMenosDeUmaHoraDepois_E_EhMenosPrioritaria_DisparaExcecao() 
        {
            var dataFimNovaReserva = dataInicioReservaExistente.AddMinutes(-1);
            var dataInicioNovaReserva = dataFimNovaReserva.AddHours(-2);            

            Evento outroEvento = CriarEvento(TipoEventoEnum.Comum, DateTime.Now, DateTime.Now.AddHours(4));
            outroEvento.Reservar(localReservado, dataInicioNovaReserva, dataFimNovaReserva);

            agendaEventoService.Agendar(outroEvento);
        }

        private void CriarEventoExistenteQueReservouLocal() 
        {
            localReservado = new Local("Um Local");
            dataInicioReservaExistente = DateTime.Now;
            dataFimReservaExistente = dataInicioReservaExistente.AddHours(1);

            Evento evento = CriarEvento(TipoEventoEnum.Comum, DateTime.Now, DateTime.Now.AddHours(4));
            evento.Reservar(localReservado, dataInicioReservaExistente, dataFimReservaExistente);

            DataBaseSession.Store(evento);
            DataBaseSession.SaveChanges();
        }

        private Evento CriarEvento(TipoEventoEnum tipo, DateTime dataInicio, DateTime dataFim)
        {
            var grupo = new Grupo("Grupo");
            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }
    }
}
