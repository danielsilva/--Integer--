using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Services;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;

namespace Integer.UnitTests.Domain.Services
{
    [TestFixture]
    public class AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_Test : InMemoryDataBaseTest
    {
        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreReservas = TimeSpan.FromMinutes(59);

        [Test]
        [Ignore("TODO")]
        public void QuandoAReservaDeLocal_ComecaMenosDeUmaHoraDepois_E_EhMenosPrioritaria_DisparaExcecao() 
        {
            Evento evento = CriarEvento(TipoEventoEnum.Paroquial, DateTime.Now, DateTime.Now.AddHours(4));
            
        }

        private Evento CriarEvento(TipoEventoEnum tipo, DateTime dataInicio, DateTime dataFim)
        {
            Grupo grupo = new Grupo("Grupo");

            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }
    }
}
