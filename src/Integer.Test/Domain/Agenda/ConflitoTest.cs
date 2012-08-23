using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Rhino.Mocks;
using Integer.Infrastructure.DateAndTime;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class ConflitoTest
    {
        Conflito conflito;
        Evento evento;
        DateTime dataAtual;

        public ConflitoTest() 
        {
            evento = MockRepository.GenerateStub<Evento>();

            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;
            conflito = new Conflito(evento, MotivoConflitoEnum.ExisteEventoParoquialNaData);
        }

        [Fact]
        public void Possui_Data_Igual_A_Data_Atual() 
        {
            Assert.Equal(SystemTime.Now(), conflito.Data);
        }

        [Fact]
        public void Mapeia_Evento() 
        {
            Assert.Equal(evento.Id, conflito.Evento.Id);
        }

        [Fact]
        public void Mapeia_Motivo() 
        {
            Assert.Equal(MotivoConflitoEnum.ExisteEventoParoquialNaData, conflito.Motivo);
        }
    }
}
