using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using DbC;
using Integer.Infrastructure.Events;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Evento_AlterarReservasDeLocais
    {
        Local local;
        DateTime dataInicioEvento, dataFimEvento;

        [Test]
        public void QuandoNaoMudaNada_ReservasPermanecemInalteradas()
        {
            Evento evento = CriarEventoComReserva();

            var reservasAlteradas = new Dictionary<Local, Horario>();
            reservasAlteradas.Add(local, new Horario(dataInicioEvento, dataFimEvento));

            evento.AlterarReservasDeLocais(reservasAlteradas);

            Assert.AreEqual(1, evento.Reservas.Count());
            var reservaEsperada = new Reserva(local, dataInicioEvento, dataFimEvento);
            Assert.AreEqual(reservaEsperada, evento.Reservas.First());
        }

        [Test]
        public void QuandoAlteraDatas_ReservaFicaComNovasDatas() 
        {
            Evento evento = CriarEventoComReserva();

            var reservasAlteradas = new Dictionary<Local, Horario>();
            reservasAlteradas.Add(local, new Horario(dataInicioEvento, dataFimEvento.AddHours(-1)));

            evento.AlterarReservasDeLocais(reservasAlteradas);

            Assert.AreEqual(1, evento.Reservas.Count());
            var reservaEsperada = new Reserva(local, dataInicioEvento, dataFimEvento.AddHours(-1));
            Assert.AreEqual(reservaEsperada, evento.Reservas.First());
        }

        [Test]
        public void QuandoAlteraDatas_DisparaEventoDeAlteracaoDeReserva() 
        {
            HorarioDeReservaDeLocalAlteradoEvent eventoDisparado = null;
            DomainEvents.Register<HorarioDeReservaDeLocalAlteradoEvent>(e => eventoDisparado = e);

            Evento evento = CriarEventoComReserva();

            var reservasAlteradas = new Dictionary<Local, Horario>();
            reservasAlteradas.Add(local, new Horario(dataInicioEvento, dataFimEvento.AddHours(-1)));
            evento.AlterarReservasDeLocais(reservasAlteradas);

            Assert.AreEqual(evento, eventoDisparado.Evento);
            Assert.AreEqual(evento.Reservas.First(), eventoDisparado.Reserva);
        }

        private Evento CriarEventoComReserva()
        {
            dataInicioEvento = DateTime.Now;
            dataFimEvento = dataInicioEvento.AddHours(4);
            var grupo = MockRepository.GenerateStub<Grupo>();
            var evento = new Evento("Nome", "Descricao", dataInicioEvento, dataFimEvento, grupo, TipoEventoEnum.Comum);

            local = MockRepository.GenerateStub<Local>();
            evento.Reservar(local, dataInicioEvento, dataFimEvento);

            return evento;
        }
    }
}
