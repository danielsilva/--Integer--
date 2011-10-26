using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;

namespace Integer.UnitTests.Domain.Paroquia
{
    [TestFixture]
    public class Local_Novo_Validacoes
    {
        string nome;
        Local local;

        private void Cria_Local() 
        {
            local = new Local(nome);
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeEhNulo_DisparaExcecao()
        {
            nome = null;
            Cria_Local();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeEhVazio_DisparaExcecao()
        {
            nome = " ";
            Cria_Local();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeTemMaisDe50Caracteres_DisparaExcecao()
        {
            var nomeMaiorQue50 = new StringBuilder();
            nomeMaiorQue50.Length = 51;
            nome = nomeMaiorQue50.ToString();

            Cria_Local();
        }
    }
}
