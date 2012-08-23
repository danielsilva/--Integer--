using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_RemoverConflito
    {
        [Fact]
        public void QuandoRemoveUltimoConflito_EventoFicaComEstadoAgendado() 
        {
            Evento evento = CriarEvento();
            Evento outroEvento = CriarEvento();
            evento.AdicionarConflito(outroEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);

            evento.RemoverConflitoCom(outroEvento);
            Assert.Equal(EstadoEventoEnum.Agendado, evento.Estado);
        }

        [Fact]
        public void QuandoRemoveConflito_E_AindaPossuiConflitos_EventoFicaComEstadoNaoAgendado()
        {
            Evento evento = CriarEvento();
            Evento outroEvento = CriarEvento();
            outroEvento.Id = "1";
            Evento maisOutroEvento = CriarEvento();
            maisOutroEvento.Id = "2";
            evento.AdicionarConflito(outroEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            evento.AdicionarConflito(maisOutroEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);

            evento.RemoverConflitoCom(outroEvento);
            Assert.Equal(EstadoEventoEnum.NaoAgendado, evento.Estado);
        }

        private Evento CriarEvento()
        {
            Grupo grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", null, DateTime.Now, DateTime.Now.AddHours(1), grupo, TipoEventoEnum.Comum);
        }
    }
}
