using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Evento_RemoverConflito
    {
        [Test]
        public void QuandoRemoveUltimoConflito_EventoFicaComEstadoAgendado() 
        {
            Evento evento = CriarEvento();
            Evento outroEvento = CriarEvento();
            evento.AdicionarConflito(outroEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);

            evento.RemoverConflitoCom(outroEvento);
            Assert.AreEqual(EstadoEventoEnum.Agendado, evento.Estado);
        }

        [Test]
        public void QuandoRemoveConflito_E_AindaPossuiConflitos_EventoFicaComEstadoNaoAgendado()
        {
            Evento evento = CriarEvento();
            Evento outroEvento = CriarEvento();
            Evento maisOutroEvento = CriarEvento();
            evento.AdicionarConflito(outroEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            evento.AdicionarConflito(maisOutroEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);

            evento.RemoverConflitoCom(outroEvento);
            Assert.AreEqual(EstadoEventoEnum.NaoAgendado, evento.Estado);
        }

        private Evento CriarEvento()
        {
            Grupo grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", null, DateTime.Now, DateTime.Now.AddHours(1), grupo, TipoEventoEnum.Comum);
        }
    }
}
