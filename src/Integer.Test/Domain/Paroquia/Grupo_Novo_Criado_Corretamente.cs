using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Paroquia
{
    public class Grupo_Novo_Criado_Corretamente
    {
        Grupo grupo, grupoPai;

        public Grupo_Novo_Criado_Corretamente() 
        {
            string nome = "Grupo";
            string email = "grupo@Paroquia.com.br";
            grupoPai = MockRepository.GenerateStub<Grupo>();

            grupo = new Grupo(nome, email, grupoPai);
        }

        [Fact]
        public void Mapeia_Nome() 
        {
            Assert.Equal("Grupo", grupo.Nome);
        }

        [Fact]
        public void Mapeia_Email()
        {
            Assert.Equal("grupo@Paroquia.com.br", grupo.Email);
        }

        [Fact]
        public void Mapeia_GrupoPai() 
        {
            Assert.Equal(grupoPai.Id, grupo.GrupoPai.Id);
        }

        [Fact]
        public void PrecisaTrocarSenha() 
        {
            Assert.True(grupo.PrecisaCriarUsuario);
        }

        [Fact]
        public void Senha_EhPadrao_calendario2013() 
        {
            Assert.Equal("calendario2013", grupo.SenhaDescriptografada);
        }
    }
}
