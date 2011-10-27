using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Domain.Agenda;
using Integer.Infrastructure.DateAndTime;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Reserva_Nova_Criada_Corretamente
    {
        Reserva reserva;
        Local local;
        DateTime dataInicio, dataFim;

        [TestFixtureSetUp]
        public void Setup() 
        {
            local = new Local("Um Local");
            dataInicio = DateTime.Now;
            dataFim = dataInicio.AddHours(2);

            reserva = new Reserva(local, dataInicio, dataFim);
        }

        [Test]
        public void Mapeia_Local() 
        {
            Assert.AreEqual(local, reserva.Local);
        }

        [Test]
        public void Mapeia_DataInicio()
        {
            Assert.AreEqual(dataInicio, reserva.DataInicio);
        }

        [Test]
        public void Mapeia_DataFim()
        {
            Assert.AreEqual(dataFim, reserva.DataFim);
        }

        [Test]
        public void Mapeia_Horario() 
        {
            var horarioEsperado = new Horario(dataInicio, dataFim);
            Assert.AreEqual(horarioEsperado, reserva.Horario);
        }
    }
}
