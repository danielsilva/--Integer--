using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Reserva_Nova_Validacoes
    {
        Reserva reserva;
        Local local;
        DateTime dataInicio, dataFim;

        public Reserva_Nova_Validacoes() 
        {
            local = MockRepository.GenerateStub<Local>();
            dataInicio = DateTime.Now;
            dataFim = dataInicio.AddHours(2);
        }

        private void Cria_Reserva() 
        {
            reserva = new Reserva(local, dataInicio, dataFim);
        }

        [Fact]
        public void QuandoLocalEhNulo_DisparaExcecao()
        {
            local = null;
            Assert.Throws<DbCException>(() => Cria_Reserva());
        }

        [Fact]
        public void QuandoDataInicioNaoExiste_DisparaExcecao() 
        {
            dataInicio = default(DateTime);
            Assert.Throws<DbCException>(() => Cria_Reserva());
        }

        [Fact]
        public void QuandoDataFimNaoExiste_DisparaExcecao()
        {
            dataFim = default(DateTime);
            Assert.Throws<DbCException>(() => Cria_Reserva());
        }

        [Fact]
        public void QuandoDataInicioEhPosteriorADataFim_DisparaExcecao() 
        {
            dataInicio = dataFim.AddHours(1);
            Assert.Throws<DbCException>(() => Cria_Reserva());
        }

        [Fact]
        public void QuandoDataInicioEhIgualADataFim_DisparaExcecao()
        {
            dataInicio = dataFim;
            Assert.Throws<DbCException>(() => Cria_Reserva());
        }
    }
}
