using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using Integer.Domain.Agenda;
using Integer.Infrastructure.DateAndTime;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Reserva_Nova_Criada_Corretamente
    {
        Reserva reserva;
        Local local;
        DateTime dataInicio, dataFim;

        public Reserva_Nova_Criada_Corretamente() 
        {
            local = new Local("Um Local");
            dataInicio = DateTime.Now;
            dataFim = dataInicio.AddHours(2);

            reserva = new Reserva(local, dataInicio, dataFim);
        }

        [Fact]
        public void Mapeia_Local() 
        {
            Assert.Equal(local.Id, reserva.Local.Id);
        }

        [Fact]
        public void Mapeia_DataInicio()
        {
            Assert.Equal(dataInicio, reserva.DataInicio);
        }

        [Fact]
        public void Mapeia_DataFim()
        {
            Assert.Equal(dataFim, reserva.DataFim);
        }

        [Fact]
        public void Mapeia_Horario() 
        {
            var horarioEsperado = new Horario(dataInicio, dataFim);
            Assert.Equal(horarioEsperado, reserva.Horario);
        }
    }
}
