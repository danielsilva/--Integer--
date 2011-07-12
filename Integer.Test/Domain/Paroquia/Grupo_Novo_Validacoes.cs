using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;

namespace Integer.UnitTests.Domain.Paroquia
{
    [TestFixture]
    public class Grupo_Novo_Validacoes
    {
        string nome;
        Grupo grupo;

        [SetUp]
        public void Setup() 
        {
            nome = "Retiro Salvatoriano";
        }

        private void Cria_Grupo() 
        {
            grupo = new Grupo(nome);
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Nome_Eh_Nulo()
        {
            nome = null;
            Cria_Grupo();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Nome_Eh_Vazio()
        {
            nome = " ";
            Cria_Grupo();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Nome_Tem_Mais_De_50_Caracteres()
        {
            var nomeMaiorQue50 = new StringBuilder();
            nomeMaiorQue50.Length = 51;
            nome = nomeMaiorQue50.ToString();

            Cria_Grupo();
        }
    }
}
