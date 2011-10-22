using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Evento_AdicionarConflito
    {
        Evento evento, outroEvento;

        [TestFixtureSetUp]
        public void init() 
        {
            evento = MockRepository.GenerateStub<Evento>();
            outroEvento = MockRepository.GenerateStub<Evento>();

            evento.AdicionarConflito(outroEvento, MotivoConflitoEnum.ExisteEventoParoquialNaData);
        }

        [Test]
        public void EventoPossuiEstado_NaoAgendado() 
        {
            Assert.AreEqual(EstadoEventoEnum.NaoAgendado, evento.Estado);
        }

        [Test]
        public void EventoPossuiUmConflitoReferenteAoOutroEvento() 
        {
            Assert.AreEqual(1, evento.Conflitos.Count());
            Assert.AreEqual(outroEvento, evento.Conflitos.Single().Evento);
        }
    }
}
