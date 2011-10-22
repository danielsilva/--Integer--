using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Services;

namespace Integer.UnitTests.Domain.Services
{
    [TestFixture]
    public class AgendaEventoService_QuandoLocalJaEstaReservadoParaOutroEvento_Test : InMemoryDataBaseTest
    {
        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreReservas = TimeSpan.FromMinutes(59);


    }
}
