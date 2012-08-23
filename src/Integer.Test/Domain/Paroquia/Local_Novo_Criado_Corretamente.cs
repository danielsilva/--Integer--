using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using Xunit;

namespace Integer.UnitTests.Domain.Paroquia
{
    public class Local_Novo_Criado_Corretamente
    {
        Local local;

        public Local_Novo_Criado_Corretamente() 
        {
            string nome = "Um Local";
            local = new Local(nome);
        }

        [Fact]
        public void Mapeia_Nome() 
        {
            Assert.Equal("Um Local", local.Nome);
        }
    }
}
