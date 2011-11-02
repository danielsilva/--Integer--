using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Services
{
    [TestFixture]
    public class RemoveConflitoServiceTest : InMemoryDataBaseTest
    {
        [Test]
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
