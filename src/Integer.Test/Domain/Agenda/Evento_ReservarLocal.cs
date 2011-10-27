using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using DbC;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Evento_ReservarLocal
    {
        [SetUp]
        public void init() 
        {

        }

        [Test]
        public void AoReservarComSucesso_ReservaEstahAssociadoAEvento() 
        {
            Local local = MockRepository.GenerateStub<Local>();

            DateTime dataInicioEvento = DateTime.Now;
            DateTime dataFimEvento = dataInicioEvento.AddHours(4);
            Evento evento = CriarEvento(dataInicioEvento, dataFimEvento);
            evento.Reservar(local, dataInicioEvento, dataFimEvento);

            Assert.AreEqual(1, evento.Reservas.Count());
            Reserva reservaEsperada = new Reserva(local, dataInicioEvento, dataFimEvento);
            Assert.AreEqual(reservaEsperada, evento.Reservas.First());
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoJaExisteReservaDeUmLocalParaOutroHorarioParecido_DisparaExcecao() 
        {
            Local local = MockRepository.GenerateStub<Local>();

            DateTime dataInicioEvento = DateTime.Now;
            DateTime dataFimEvento = dataInicioEvento.AddHours(4);
            Evento evento = CriarEvento(dataInicioEvento, dataFimEvento);

            evento.Reservar(local, dataInicioEvento, dataFimEvento);
            evento.Reservar(local, dataInicioEvento, dataFimEvento.AddHours(1));
        }

        private Evento CriarEvento(DateTime dataInicio, DateTime dataFim)
        {
            var grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, TipoEventoEnum.Comum);
        }
    }
}
