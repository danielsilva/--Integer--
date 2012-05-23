using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Paroquia
{
    [TestFixture]
    public class Grupo_Novo_Criado_Corretamente
    {
        Grupo grupo, grupoPai;

        [TestFixtureSetUp]
        public void Setup() 
        {
            string nome = "Grupo";
            string email = "grupo@Paroquia.com.br";
            grupoPai = MockRepository.GenerateStub<Grupo>();
            string cor = "cor";

            grupo = new Grupo(nome, email, grupoPai, cor);
        }

        [Test]
        public void Mapeia_Nome() 
        {
            Assert.AreEqual("Grupo", grupo.Nome);
        }

        [Test]
        public void Mapeia_Email()
        {
            Assert.AreEqual("grupo@Paroquia.com.br", grupo.Email);
        }

        [Test]
        public void Mapeia_GrupoPai() 
        {
            Assert.AreEqual(grupoPai.Id, grupo.GrupoPai.Id);
        }

        [Test]
        public void Mapeia_Cor() 
        {
            Assert.AreEqual("cor", grupo.CorNoCalendario);
        }

        [Test]
        public void PrecisaTrocarSenha() 
        {
            Assert.IsTrue(grupo.PrecisaTrocarSenha);
        }

        [Test]
        public void Senha_EhPadrao_calendario2012() 
        {
            Assert.AreEqual("calendario2012", grupo.SenhaDescriptografada);
        }
    }
}
