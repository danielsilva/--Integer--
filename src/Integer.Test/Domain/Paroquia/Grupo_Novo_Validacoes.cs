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

namespace Integer.UnitTests.Domain.Paroquia
{
    public class Grupo_Novo_Validacoes
    {
        string nome;
        string email;
        Grupo grupo, grupoPai;

        public Grupo_Novo_Validacoes()
        {
            nome = "Grupo";
            email = "email@email.com";
            grupoPai = MockRepository.GenerateStub<Grupo>();
        }

        [Fact]
        public void QuandoNomeEhNulo_DisparaExcecao()
        {
            nome = null;
            Assert.Throws<DbCException>(() => Cria_Grupo());
        }

        [Fact]
        public void QuandoNomeEhVazio_DisparaExcecao()
        {
            nome = " ";
            Assert.Throws<DbCException>(() => Cria_Grupo());
        }

        [Fact]
        public void QuandoNomeTemMaisDe50Caracteres_DisparaExcecao()
        {
            var nomeMaiorQue50 = new StringBuilder();
            nomeMaiorQue50.Length = 51;
            nome = nomeMaiorQue50.ToString();

            Assert.Throws<DbCException>(() => Cria_Grupo());
        }

        private void Cria_Grupo()
        {
            grupo = new Grupo(nome, email, grupoPai);
        }
    }
}
