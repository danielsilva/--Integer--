using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using Rhino.Mocks;
using Integer.Infrastructure.DateAndTime;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class ConflitoTest
    {
        Conflito conflito;
        Evento evento;
        DateTime dataAtual;

        [TestFixtureSetUp]
        public void init() 
        {
            evento = MockRepository.GenerateStub<Evento>();

            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;
            conflito = new Conflito(evento, MotivoConflitoEnum.ExisteEventoParoquialNaData);
        }

        [Test]
        public void Possui_Data_Igual_A_Data_Atual() 
        {
            Assert.AreEqual(SystemTime.Now(), conflito.Data);
        }

        [Test]
        public void Mapeia_Evento() 
        {
            Assert.AreEqual(evento, conflito.Evento);
        }

        [Test]
        public void Mapeia_Motivo() 
        {
            Assert.AreEqual(MotivoConflitoEnum.ExisteEventoParoquialNaData, conflito.Motivo);
        }
    }
}
