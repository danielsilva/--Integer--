using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using Integer.Infrastructure.DateAndTime;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Evento_Novo_Criado_Corretamente
    {
        Evento evento;
        DateTime dataAtual, dataInicioEvento, dataFimEvento;

        [TestFixtureSetUp]
        public void Setup() 
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

        [Test]
        public void Possui_DataCadastro_Igual_A_Data_Atual() 
        {
            Assert.AreEqual(dataAtual.ToString("dd/MM/yyyy hh:mm"), evento.DataCadastro.ToString("dd/MM/yyyy hh:mm"));
        }

        [Test]
        public void Possui_Estado_Agendado() 
        {
            Assert.AreEqual(EstadoEventoEnum.Agendado, evento.Estado);
        }

        [Test]
        public void Mapeia_Nome() 
        {
            Assert.AreEqual("Retiro Salvatoriano", evento.Nome);
        }

        [Test]
        public void Mapeia_Descricao() 
        {
            Assert.AreEqual("Retiro de aprofundamento da espiritualidade Salvatoriana", evento.Descricao);
        }

        [Test]
        public void Mapeia_DataInicio() 
        {
            Assert.AreEqual(dataInicioEvento, evento.DataInicio);
        }

        [Test]
        public void Mapeia_DataFim()
        {
            Assert.AreEqual(dataFimEvento, evento.DataFim);
        }

        [Test]
        public void Mapeia_Grupo()
        {
            Assert.AreEqual("IdGrupo", evento.Grupo.Id);
        }

        [Test]
        public void Mapeia_TipoEvento()
        {
            Assert.AreEqual(TipoEventoEnum.Comum, evento.Tipo);
        }
    }
}
