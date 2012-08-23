using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using Integer.Infrastructure.DateAndTime;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_Novo_Criado_Corretamente
    {
        Evento evento;
        DateTime dataAtual, dataInicioEvento, dataFimEvento;

        public Evento_Novo_Criado_Corretamente() 
        {
            string nome = "Retiro Salvatoriano";
            string descricao = "Retiro de aprofundamento da espiritualidade Salvatoriana";
            dataInicioEvento = new DateTime(2011, 01, 01, 8, 0, 0);
            dataFimEvento = new DateTime(2011, 01, 01, 10, 0, 0);
            TipoEventoEnum tipoDoEvento = TipoEventoEnum.Comum;

            Grupo grupo = MockRepository.GenerateStub<Grupo>();
            grupo.Id = "IdGrupo";

            dataAtual = DateTime.Now;
            SystemTime.Now = () => dataAtual;
            evento = new Evento(nome, descricao, dataInicioEvento, dataFimEvento, grupo, tipoDoEvento);
        }

        [Fact]
        public void Possui_DataCadastro_Igual_A_Data_Atual() 
        {
            Assert.Equal(dataAtual.ToString("dd/MM/yyyy hh:mm"), evento.DataCadastro.ToString("dd/MM/yyyy hh:mm"));
        }

        [Fact]
        public void Possui_Estado_Agendado() 
        {
            Assert.Equal(EstadoEventoEnum.Agendado, evento.Estado);
        }

        [Fact]
        public void Mapeia_Nome() 
        {
            Assert.Equal("Retiro Salvatoriano", evento.Nome);
        }

        [Fact]
        public void Mapeia_Descricao() 
        {
            Assert.Equal("Retiro de aprofundamento da espiritualidade Salvatoriana", evento.Descricao);
        }

        [Fact]
        public void Mapeia_DataInicio() 
        {
            Assert.Equal(dataInicioEvento, evento.DataInicio);
        }

        [Fact]
        public void Mapeia_DataFim()
        {
            Assert.Equal(dataFimEvento, evento.DataFim);
        }

        [Fact]
        public void Mapeia_Grupo()
        {
            Assert.Equal("IdGrupo", evento.Grupo.Id);
        }

        [Fact]
        public void Mapeia_TipoEvento()
        {
            Assert.Equal(TipoEventoEnum.Comum, evento.Tipo);
        }
    }
}
