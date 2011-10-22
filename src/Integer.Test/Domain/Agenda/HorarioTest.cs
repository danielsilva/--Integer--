using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class HorarioTest
    {
        [Test]
        public void QuandoComecaETerminaNoMesmoDia_SeparaSomenteAHora() 
        {
            var dataInicio = new DateTime(2011, 2, 1, 8, 0, 0);
            var dataFim = dataInicio.AddHours(2);

            var horario = new Horario(dataInicio, dataFim);

            Assert.AreEqual("01/02/2011 08:00 até 10:00", horario.ToString());
        }

        [Test]
        public void QuandoTerminaEmOutroDia_SeparaDiaEHora() 
        {
            var dataInicio = new DateTime(2011, 2, 1, 8, 0, 0);
            var dataFim = dataInicio.AddDays(1);

            var horario = new Horario(dataInicio, dataFim);

            Assert.AreEqual("01/02/2011 08:00 até 02/02/2011 08:00", horario.ToString());
        }
    }
}
