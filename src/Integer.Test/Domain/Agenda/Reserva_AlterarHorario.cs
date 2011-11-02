using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Reserva_AlterarHorario
    {
        Reserva reserva;
        Local local;
        DateTime dataInicio, dataFim;

        [SetUp]
        public void init() 
        {
            local = MockRepository.GenerateStub<Local>();
            dataInicio = DateTime.Now;
            dataFim = dataInicio.AddHours(2);

            reserva = new Reserva(local, dataInicio, dataFim);
        }

        [Test]
        public void QuandoAlteraDataInicio_MapeiaCorretamente() 
        {
            var novaDataInicio = dataInicio.AddMinutes(10);
            reserva.AlterarHorario(new Horario(novaDataInicio, dataFim));

            Assert.AreEqual(novaDataInicio, reserva.DataInicio);
        }

        [Test]
        public void QuandoAlteraDataFim_MapeiaCorretamente()
        {
            var novaDataFim = dataFim.AddMinutes(10);
            reserva.AlterarHorario(new Horario(dataInicio, novaDataFim));

            Assert.AreEqual(novaDataFim, reserva.DataFim);
        }
    }
}
