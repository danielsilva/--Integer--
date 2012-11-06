using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Rhino.Mocks;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Repository;
using Xunit;

namespace Integer.UnitTests.Domain.Services
{
    public class AgendaEventoServiceTest : InMemoryDataBaseTest
    {
        [Fact]
        public void AgendaComSucesso() 
        {
            Eventos eventos = new EventoRepository(DataBaseSession);
            AgendaEventoService agendaService = new AgendaEventoService(eventos);

            Evento novoEvento = CriarEvento();
            agendaService.Agendar(novoEvento);
            DataBaseSession.SaveChanges();
            
            Assert.Equal(1, DataBaseSession.Query<Evento>().Count());
        }

        private Evento CriarEvento()
        {
            DateTime dataInicio, dataFim;
            dataInicio = DateTime.Now;
            dataFim = dataInicio.AddHours(2);

            Grupo grupo = MockRepository.GenerateStub<Grupo>();
            grupo.Nome = "Grupo";

            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, TipoEventoEnum.Comum);
        }
    }
}
