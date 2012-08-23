using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Reserva_AlterarHorario
    {
        Reserva reserva;
        Local local;
        DateTime dataInicio, dataFim;

        public Reserva_AlterarHorario() 
        {
            local = MockRepository.GenerateStub<Local>();
            dataInicio = DateTime.Now;
            dataFim = dataInicio.AddHours(2);

            reserva = new Reserva(local, dataInicio, dataFim);
        }

        [Fact]
        public void QuandoAlteraDataInicio_MapeiaCorretamente() 
        {
            var novaDataInicio = dataInicio.AddMinutes(10);
            reserva.AlterarHorario(new Horario(novaDataInicio, dataFim));

            Assert.Equal(novaDataInicio, reserva.DataInicio);
        }

        [Fact]
        public void QuandoAlteraDataFim_MapeiaCorretamente()
        {
            var novaDataFim = dataFim.AddMinutes(10);
            reserva.AlterarHorario(new Horario(dataInicio, novaDataFim));

            Assert.Equal(novaDataFim, reserva.DataFim);
        }
    }
}
