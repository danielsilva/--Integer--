using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Reserva_Nova_Validacoes
    {
        Reserva reserva;
        Local local;
        DateTime dataInicio, dataFim;

        [SetUp]
        public void Setup() 
        {
            local = MockRepository.GenerateStub<Local>();
            dataInicio = DateTime.Now;
            dataFim = dataInicio.AddHours(2);
        }

        private void Cria_Reserva() 
        {
            reserva = new Reserva(local, dataInicio, dataFim);
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoLocalEhNulo_DisparaExcecao()
        {
            local = null;
            Cria_Reserva();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoDataInicioNaoExiste_DisparaExcecao() 
        {
            dataInicio = default(DateTime);
            Cria_Reserva();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoDataFimNaoExiste_DisparaExcecao()
        {
            dataFim = default(DateTime);
            Cria_Reserva();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoDataInicioEhPosteriorADataFim_DisparaExcecao() 
        {
            dataInicio = dataFim.AddHours(1);
            Cria_Reserva();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoDataInicioEhIgualADataFim_DisparaExcecao()
        {
            dataInicio = dataFim;
            Cria_Reserva();
        }
    }
}
