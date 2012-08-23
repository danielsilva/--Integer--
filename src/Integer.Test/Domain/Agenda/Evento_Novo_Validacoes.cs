using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Rhino.Mocks;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_Novo_Validacoes
    {
        string nome, descricao;
        DateTime dataInicioEvento, dataFimEvento;
        Grupo grupo;
        TipoEventoEnum tipoDoEvento;

        Evento evento;

        public Evento_Novo_Validacoes() 
        {
            nome = "Retiro Salvatoriano";
            descricao = "Retiro de aprofundamento da espiritualidade Salvatoriana";
            dataInicioEvento = new DateTime(2011, 01, 01, 8, 0, 0);
            dataFimEvento = new DateTime(2011, 01, 01, 10, 0, 0);
            grupo = MockRepository.GenerateStub<Grupo>();
            tipoDoEvento = TipoEventoEnum.Comum;
        }

        private void Cria_Evento() 
        {
            evento = new Evento(nome, descricao, dataInicioEvento, dataFimEvento, grupo, tipoDoEvento);
        }

        [Fact]
        public void QuandoNomeEhNulo_DisparaExcecao()
        {
            nome = null;
            Assert.Throws<DbCException>(() =>Cria_Evento());
        }

        [Fact]
        public void QuandoNomeEhVazio_DisparaExcecao()
        {
            nome = " ";
            Assert.Throws<DbCException>(() => Cria_Evento());
        }

        [Fact]
        public void QuandoNomeTemMaisDe50Caracteres_DisparaExcecao()
        {
            var nomeMaiorQue50 = new StringBuilder();
            nomeMaiorQue50.Length = 51;
            nome = nomeMaiorQue50.ToString();

            Assert.Throws<DbCException>(() => Cria_Evento());
        }

        [Fact]
        public void QuandoDescricaoTemMaisDe150Caracteres_DisparaExcecao()
        {
            var descricaoMaiorQue150 = new StringBuilder();
            descricaoMaiorQue150.Length = 151;
            descricao = descricaoMaiorQue150.ToString();

            Assert.Throws<DbCException>(() => Cria_Evento());
        }

        [Fact]
        public void QuandoDataInicioEhPosteriorADataFim_DisparaExcecao()
        {
            dataInicioEvento = new DateTime(2011, 01, 01, 10, 00, 00);
            dataFimEvento = new DateTime(2011, 01, 01, 08, 00, 00);

            Assert.Throws<DbCException>(() => Cria_Evento());
        }

        [Fact]
        public void QuandoDataInicioNaoInformada_DisparaExcecao()
        {
            dataInicioEvento = default(DateTime);

            Assert.Throws<DbCException>(() => Cria_Evento());
        }

        [Fact]
        public void QuandoDataFimNaoInformada_DisparaExcecao()
        {
            dataFimEvento = default(DateTime);

            Assert.Throws<DbCException>(() => Cria_Evento());
        }

        [Fact]
        public void QuandoGrupoNaoInformado_DisparaExcecao()
        {
            grupo = null;

            Assert.Throws<DbCException>(() => Cria_Evento());
        }

        [Fact]
        public void QuandoTipoNaoInformado_DisparaExcecao()
        {
            tipoDoEvento = default(TipoEventoEnum);

            Assert.Throws<DbCException>(() => Cria_Evento());
        }
    }
}
