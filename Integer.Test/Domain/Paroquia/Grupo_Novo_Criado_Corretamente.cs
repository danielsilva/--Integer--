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
            grupo = new Grupo(nome);
        }

        [Test]
        public void Mapeia_Nome_Corretamente() 
        {
            Assert.AreEqual("Grupo", grupo.Nome);
        }
    }
}
