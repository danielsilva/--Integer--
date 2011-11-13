using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Paroquia;

namespace Integer.UnitTests.Domain.Paroquia
{
    [TestFixture]
    public class Grupo_Novo_Criado_Corretamente
    {
        Grupo grupo;

        [TestFixtureSetUp]
        public void Setup() 
        {
            string nome = "Grupo";
            string email = "grupo@Paroquia.com.br";
            grupo = new Grupo(nome, email);
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
    }
}
