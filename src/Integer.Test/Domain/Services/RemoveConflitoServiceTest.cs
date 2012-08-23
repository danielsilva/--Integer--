using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Services
{
    public class RemoveConflitoServiceTest : InMemoryDataBaseTest
    {
        [Fact]
        public void a() 
        {
            Evento evento = CriarEvento();
            Evento outroEvento = CriarEvento();
            Evento maisOutroEvento = CriarEvento();
            evento.AdicionarConflito(outroEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            evento.AdicionarConflito(maisOutroEvento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);

            DataBaseSession.Store(evento);
        }

        private Evento CriarEvento()
        {
            Grupo grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", null, DateTime.Now, DateTime.Now.AddHours(1), grupo, TipoEventoEnum.Comum);
        }
    }
}
