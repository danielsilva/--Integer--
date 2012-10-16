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
        DateTime data;

        public Reserva_AlterarHorario() 
        {
            local = MockRepository.GenerateStub<Local>();
            data = DateTime.Now.Date;

            reserva = new Reserva(local, data, new List<HoraReservaEnum> { HoraReservaEnum.Manha });
        }

        [Fact]
        public void QuandoAlteraHora_MapeiaCorretamente() 
        {
            reserva.AlterarHorario(data.AddDays(1), new List<HoraReservaEnum> { HoraReservaEnum.Tarde });

            Assert.Equal(data.AddDays(1), reserva.Data);
            Assert.Equal(HoraReservaEnum.Tarde, reserva.Hora.Single());
        }
    }
}
