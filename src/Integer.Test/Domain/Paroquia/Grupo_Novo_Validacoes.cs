﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Validation;
using DbC;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Paroquia
{
    [TestFixture]
    public class Grupo_Novo_Validacoes
    {
        string nome;
        string email;
        Grupo grupo, grupoPai;
        string cor;

        [SetUp]
        public void Setup()
        {
            nome = "Grupo";
            email = "email@email.com";
            grupoPai = MockRepository.GenerateStub<Grupo>();
            cor = "cor";
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeEhNulo_DisparaExcecao()
        {
            nome = null;
            Cria_Grupo();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeEhVazio_DisparaExcecao()
        {
            nome = " ";
            Cria_Grupo();
        }

        [Test]
        [ExpectedException(typeof(DbCException))]
        public void QuandoNomeTemMaisDe50Caracteres_DisparaExcecao()
        {
            var nomeMaiorQue50 = new StringBuilder();
            nomeMaiorQue50.Length = 51;
            nome = nomeMaiorQue50.ToString();

            Cria_Grupo();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuandoGrupoPaiEhNulo_DisparaExcecao() 
        {
            grupoPai = null;
            Cria_Grupo();
        }

        private void Cria_Grupo()
        {
            grupo = new Grupo(nome, email, grupoPai, cor);
        }
    }
}
