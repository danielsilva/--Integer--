using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;

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
            grupo = new Grupo("Conselho Pastoral Paroquial", null);
            tipoDoEvento = TipoEventoEnum.Comum;
        }

        private void Cria_Evento() 
        {
            evento = new Evento(nome, descricao, dataInicioEvento, dataFimEvento, grupo, tipoDoEvento);
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeEhNulo_DisparaExcecao()
        {
            nome = null;
            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeEhVazio_DisparaExcecao()
        {
            nome = " ";
            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeTemMaisDe50Caracteres_DisparaExcecao()
        {
            var nomeMaiorQue50 = new StringBuilder();
            nomeMaiorQue50.Length = 51;
            nome = nomeMaiorQue50.ToString();

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoDescricaoTemMaisDe150Caracteres_DisparaExcecao()
        {
            var descricaoMaiorQue150 = new StringBuilder();
            descricaoMaiorQue150.Length = 151;
            descricao = descricaoMaiorQue150.ToString();

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoDataInicioEhPosteriorADataFim_DisparaExcecao()
        {
            dataInicioEvento = new DateTime(2011, 01, 01, 10, 00, 00);
            dataFimEvento = new DateTime(2011, 01, 01, 08, 00, 00);

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoDataInicioNaoInformada_DisparaExcecao()
        {
            dataInicioEvento = default(DateTime);

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoDataFimNaoInformada_DisparaExcecao()
        {
            dataFimEvento = default(DateTime);

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoGrupoNaoInformado_DisparaExcecao()
        {
            grupo = null;

            Cria_Evento();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoTipoNaoInformado_DisparaExcecao()
        {
            tipoDoEvento = default(TipoEventoEnum);

            Cria_Evento();
        }
    }
}
