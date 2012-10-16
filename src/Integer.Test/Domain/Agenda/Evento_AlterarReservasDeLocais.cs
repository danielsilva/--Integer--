using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using DbC;
using Integer.Infrastructure.Events;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_AlterarReservasDeLocais
    {
        Local local;
        DateTime dataInicioEvento, dataFimEvento;

        [Fact]
        public void QuandoNaoMudaNada_ReservasPermanecemInalteradas()
        {
            Evento evento = CriarEventoComReserva();

            var reservasAlteradas = new List<Reserva> { new Reserva(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Manha }) };
            evento.AlterarReservasDeLocais(reservasAlteradas);

            Assert.Equal(1, evento.Reservas.Count());
            var reservaEsperada = new Reserva(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Manha });
            Assert.Equal(reservaEsperada, evento.Reservas.First());
        }

        [Fact]
        public void QuandoAlteraHorario_ReservaFicaComNovoHorario() 
        {
            Evento evento = CriarEventoComReserva();

            var reservasAlteradas = new List<Reserva> { new Reserva(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Tarde }) };
            evento.AlterarReservasDeLocais(reservasAlteradas);

            Assert.Equal(1, evento.Reservas.Count());
            var reservaEsperada = new Reserva(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Tarde });
            Assert.Equal(reservaEsperada, evento.Reservas.First());
        }

        [Fact]
        public void QuandoAlteraDatas_DisparaEventoDeAlteracaoDeReserva() 
        {
            HorarioDeReservaDeLocalAlteradoEvent eventoDisparado = null;
            DomainEvents.Register<HorarioDeReservaDeLocalAlteradoEvent>(e => eventoDisparado = e);

            Evento evento = CriarEventoComReserva();

            var reservasAlteradas = new List<Reserva> { new Reserva(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Tarde }) };
            evento.AlterarReservasDeLocais(reservasAlteradas);

            Assert.Equal(evento, eventoDisparado.Evento);
            Assert.Equal(evento.Reservas, eventoDisparado.ReservasAlteradas);
        }

        private Evento CriarEventoComReserva()
        {
            dataInicioEvento = DateTime.Now;
            dataFimEvento = dataInicioEvento.AddHours(4);
            var grupo = MockRepository.GenerateStub<Grupo>();
            var evento = new Evento("Nome", "Descricao", dataInicioEvento, dataFimEvento, grupo, TipoEventoEnum.Comum);

            local = new Local("Local");
            local.Id = "1";

            evento.Reservar(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Manha });

            return evento;
        }
    }
}
