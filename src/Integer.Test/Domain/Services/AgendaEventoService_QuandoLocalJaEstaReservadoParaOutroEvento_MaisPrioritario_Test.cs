using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.DateAndTime;
using Integer.Infrastructure.Repository;
using Rhino.Mocks;
using Xunit;
using Integer.Domain.Agenda.Exceptions;

namespace Integer.UnitTests.Domain.Services
{
    public class AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_MaisPrioritario_Test : InMemoryDataBaseTest
    {
        Evento eventoExistente, eventoNovo;

        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreReservas = TimeSpan.FromMinutes(59);

        Local localDesejado;
        DateTime dataReservaExistente;
        DateTime dataAtual;

        public AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_MaisPrioritario_Test() 
        {
            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;

            var eventos = new EventoRepository(DataBaseSession);
            agendaEventoService = new AgendaEventoService(eventos);
        }

        [Fact]
        public void QuandoAReservaNova_ConflitaComHorarioDaReservaDoEventoPrioritarioExistente_DisparaExcecao() 
        {
            CriarEventoExistenteQueReservouLocal(TipoEventoEnum.GrandeMovimentoDePessoas);

            eventoNovo = CriarEvento(TipoEventoEnum.Comum, dataAtual, dataAtual.AddHours(4));
            var dataNovaReserva = dataReservaExistente;
            
            eventoNovo.Reservar(localDesejado, dataNovaReserva, new List<HoraReservaEnum> { HoraReservaEnum.Tarde });

            Assert.Throws<LocalReservadoException>(() => agendaEventoService.Agendar(eventoNovo));
        }

        private void CriarEventoExistenteQueReservouLocal(TipoEventoEnum tipoEvento) 
        {
            eventoExistente = CriarEvento(tipoEvento, dataAtual, dataAtual.AddHours(4));

            localDesejado = new Local("Um Local");
            dataReservaExistente = dataAtual;
            eventoExistente.Reservar(localDesejado, dataReservaExistente, new List<HoraReservaEnum> { HoraReservaEnum.Tarde });

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
