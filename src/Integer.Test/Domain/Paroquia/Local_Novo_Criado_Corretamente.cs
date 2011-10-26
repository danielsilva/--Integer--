using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Paroquia;

namespace Integer.UnitTests.Domain.Paroquia
{
    [TestFixture]
    public class Local_Novo_Criado_Corretamente
    {
        Local local;

        [TestFixtureSetUp]
        public void Setup() 
        {
            string nome = "Um Local";
            local = new Local(nome);
        }

        [Test]
        public void Mapeia_Nome() 
        {
            Assert.AreEqual("Um Local", local.Nome);
        }
    }
}
