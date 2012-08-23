using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_AdicionarConflito
    {
        Evento evento, outroEvento;

        public Evento_AdicionarConflito() 
        {
            evento = MockRepository.GenerateStub<Evento>();
            outroEvento = MockRepository.GenerateStub<Evento>();

            evento.AdicionarConflito(outroEvento, MotivoConflitoEnum.ExisteEventoParoquialNaData);
        }

        [Fact]
        public void EventoPossuiEstado_NaoAgendado() 
        {
            Assert.Equal(EstadoEventoEnum.NaoAgendado, evento.Estado);
        }

        [Fact]
        public void EventoPossuiUmConflitoReferenteAoOutroEvento() 
        {
            Assert.Equal(1, evento.Conflitos.Count());
            Assert.Equal(outroEvento.Id, evento.Conflitos.Single().Evento.Id);
        }
    }
}
