using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Evento_CancelarAgendamento
    {
        [Test]
        public void QuandoCancelaAgendamento_EventoFicaComEstadoCancelado() 
        {
            Grupo grupo = MockRepository.GenerateStub<Grupo>();
            var evento = new Evento("Nome", null, DateTime.Now, DateTime.Now.AddHours(1), grupo, TipoEventoEnum.Comum);

            evento.CancelarAgendamento();

            Assert.AreEqual(EstadoEventoEnum.Cancelado, evento.Estado);
        }
    }
}
