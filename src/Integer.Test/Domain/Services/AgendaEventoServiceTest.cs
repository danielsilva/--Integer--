using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Agenda;
using Rhino.Mocks;
using Integer.Domain.Paroquia;
using Integer.Domain.Services;
using Integer.Infrastructure.Repository;

namespace Integer.UnitTests.Domain.Services
{
    [TestFixture]
    public class AgendaEventoServiceTest : InMemoryDataBaseTest
    {
        [Test]
        public void AgendaComSucesso() 
        {
            Eventos eventos = new EventoRepository(DataBaseSession);
            AgendaEventoService agendaService = new AgendaEventoService(eventos);

            Evento novoEvento = CriarEvento();
            agendaService.Agendar(novoEvento);
            
            Assert.AreEqual(1, DataBaseSession.Query<Evento>().Count());
        }

        private Evento CriarEvento()
        {
            DateTime dataInicio, dataFim;
            dataInicio = DateTime.Now;
            dataFim = dataInicio.AddHours(2);

            Grupo grupo = new Grupo("Grupo");

            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, TipoEventoEnum.Comum);
        }
    }
}
