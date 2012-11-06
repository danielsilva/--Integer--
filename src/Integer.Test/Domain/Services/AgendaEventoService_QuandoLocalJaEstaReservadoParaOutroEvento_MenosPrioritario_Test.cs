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

namespace Integer.UnitTests.Domain.Services
{
    public class AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_MenosPrioritario_Test : InMemoryDataBaseTest
    {
        Evento eventoExistente, eventoNovo;

        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreReservas = TimeSpan.FromMinutes(59);

        Local localDesejado;
        DateTime dataReservaExistente;
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
        public void Salva_Evento_Novo_Com_Sucesso()
        {
            CriaReservaQueSobrepoe();

            Assert.Equal(2, DataBaseSession.Query<Evento>().Count());
        }

        [Fact]
        public void Evento_Existente_Fica_Com_Estado_Nao_Agendado()
        {
            CriaReservaQueSobrepoe();

            Assert.Equal(EstadoEventoEnum.NaoAgendado, eventoExistente.Estado);
        }

        [Fact]
        public void Evento_Existente_Fica_Com_Conflito_Referente_Ao_Evento_Novo()
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
            var dataNovaReserva = dataReservaExistente;

            eventoNovo.Reservar(localDesejado, dataNovaReserva, new List<HoraReservaEnum> { HoraReservaEnum.Manha });

            agendaEventoService.Agendar(eventoNovo);
            DataBaseSession.SaveChanges();
        }

        private void CriarEventoExistenteQueReservouLocal(TipoEventoEnum tipoEvento) 
        {
            eventoExistente = CriarEvento(tipoEvento, dataAtual, dataAtual.AddHours(4));

            localDesejado = new Local("Um Local");
            dataReservaExistente = dataAtual;

            eventoExistente.Reservar(localDesejado, dataReservaExistente, new List<HoraReservaEnum> { HoraReservaEnum.Manha });

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
