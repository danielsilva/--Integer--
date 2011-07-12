using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class Evento_Novo_Validacoes
    {
        string nome, descricao;
        DateTime dataInicioEvento, dataFimEvento;
        Grupo grupo;
        TipoEventoEnum tipoDoEvento;

        Evento evento;

        [SetUp]
        public void Setup() 
        {
            nome = "Retiro Salvatoriano";
            descricao = "Retiro de aprofundamento da espiritualidade Salvatoriana";
            dataInicioEvento = new DateTime(2011, 01, 01, 8, 0, 0);
            dataFimEvento = new DateTime(2011, 01, 01, 10, 0, 0);
            grupo = new Grupo("Conselho Pastoral Paroquial");
            tipoDoEvento = TipoEventoEnum.Comum;
        }

        private void Cria_Evento() 
        {
            evento = new Evento(nome, descricao, dataInicioEvento, dataFimEvento, grupo, tipoDoEvento);
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Nome_Eh_Nulo()
        {
            nome = null;
            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Nome_Eh_Vazio()
        {
            nome = " ";
            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Nome_Tem_Mais_De_50_Caracteres()
        {
            var nomeMaiorQue50 = new StringBuilder();
            nomeMaiorQue50.Length = 51;
            nome = nomeMaiorQue50.ToString();

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Descricao_Tem_Mais_De_150_Caracteres()
        {
            var descricaoMaiorQue150 = new StringBuilder();
            descricaoMaiorQue150.Length = 151;
            descricao = descricaoMaiorQue150.ToString();

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_DataInicio_Posterior_A_DataFim()
        {
            dataInicioEvento = new DateTime(2011, 01, 01, 10, 00, 00);
            dataFimEvento = new DateTime(2011, 01, 01, 08, 00, 00);

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_DataInicio_Nao_Informada()
        {
            dataInicioEvento = default(DateTime);

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_DataFim_Nao_Informada()
        {
            dataFimEvento = default(DateTime);

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Grupo_Nao_Informado()
        {
            grupo = null;

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Dispara_Excecao_Quando_Tipo_Nao_Informado()
        {
            tipoDoEvento = default(TipoEventoEnum);

            Cria_Evento();
        }
    }
}
