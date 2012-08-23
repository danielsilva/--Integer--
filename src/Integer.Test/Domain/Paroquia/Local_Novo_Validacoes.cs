using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Xunit;

namespace Integer.UnitTests.Domain.Paroquia
{
    public class Local_Novo_Validacoes
    {
        string nome;
        Local local;

        private void Cria_Local() 
        {
            local = new Local(nome);
        }

        [Fact]
        public void QuandoNomeEhNulo_DisparaExcecao()
        {
            nome = null;
            Assert.Throws<DbCException>(() => Cria_Local());
        }

        [Fact]
        public void QuandoNomeEhVazio_DisparaExcecao()
        {
            nome = " ";
            Assert.Throws<DbCException>(() => Cria_Local());
        }

        [Fact]
        public void QuandoNomeTemMaisDe50Caracteres_DisparaExcecao()
        {
            var nomeMaiorQue50 = new StringBuilder();
            nomeMaiorQue50.Length = 51;
            nome = nomeMaiorQue50.ToString();

            Assert.Throws<DbCException>(() => Cria_Local());
        }
    }
}
