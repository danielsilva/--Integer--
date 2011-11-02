using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using Integer.Infrastructure.Events;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Evento_CancelarAgendamento
    {
        [Test]
        public void QuandoCancelaAgendamento_EventoFicaComEstadoCancelado() 
        {
            var evento = CriarEvento();
            evento.CancelarAgendamento();

            Assert.AreEqual(EstadoEventoEnum.Cancelado, evento.Estado);
        }

        [Test]
        public void QuandoCancelaAgendamento_DisparaEventoDeDominioQueContemEventoCancelado()
        {
            Evento eventoDisparado = null;
            DomainEvents.Register<EventoCanceladoEvent>(e => eventoDisparado = e.Evento);

            var eventoCancelado = CriarEvento();
            eventoCancelado.CancelarAgendamento();

            Assert.AreEqual(eventoCancelado, eventoDisparado);
        }

        private Evento CriarEvento() 
        {
            Grupo grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", null, DateTime.Now, DateTime.Now.AddHours(1), grupo, TipoEventoEnum.Comum);
        }
    }
}
