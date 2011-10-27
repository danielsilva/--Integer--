﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Services;
using Integer.Domain.Agenda;
using Rhino.Mocks;
using Integer.Domain.Paroquia;
using Integer.Infrastructure.Repository;

namespace Integer.UnitTests.Domain.Services
{
    [TestFixture]
    public class AgendaEventoService_QuandoEventoNovoNaoParoquialConflitaComEventoParoquialExistente_Test : InMemoryDataBaseTest
    {
        AgendaEventoService agendaEventoService;
        TimeSpan intervaloMinimoEntreEventos = TimeSpan.FromMinutes(59);

        Evento eventoParoquialExistente;

        [SetUp]
        public void Init()
        {
            CriarEventoParoquialPreExistente();

            var eventos = new EventoRepository(DataBaseSession);
            agendaEventoService = new AgendaEventoService(eventos);
        }

        [Test]
        public void QuandoEventoNovoNaoParoquial_AconteceMenosDeUmaHoraDepoisDoEventoParoquial_DisparaExcecao() 
        {
            DateTime dataInicioEvento = eventoParoquialExistente.DataFim.AddMinutes(59);
            DateTime dataFimEvento = dataInicioEvento.AddHours(1);
            var eventoNovoNaoParoquial = CriarEvento(TipoEventoEnum.Comum, dataInicioEvento, dataFimEvento);

            Assert.Throws<EventoParoquialExistenteException>(() => agendaEventoService.Agendar(eventoNovoNaoParoquial));
        }

        [Test]
        public void QuandoEventoNovoNaoParoquial_AconteceMenosDeUmaHoraAntesDoEventoParoquial_DisparaExcecao()
        {
            DateTime dataFimEvento = eventoParoquialExistente.DataInicio.AddMinutes(-59);
            DateTime dataInicioEvento = dataFimEvento.AddHours(-1);
            var eventoNovoNaoParoquial = CriarEvento(TipoEventoEnum.Comum, dataInicioEvento, dataFimEvento);

            Assert.Throws<EventoParoquialExistenteException>(() => agendaEventoService.Agendar(eventoNovoNaoParoquial));
        }

        [Test]
        public void QuandoEventoNovoNaoParoquial_SobrepoeAoEventoParoquial_DisparaExcecao() 
        {
            DateTime dataInicioEvento = eventoParoquialExistente.DataInicio.AddMinutes(1);
            DateTime dataFimEvento = eventoParoquialExistente.DataFim.AddMinutes(-1);
            var novoEvento = CriarEvento(TipoEventoEnum.Comum, dataInicioEvento, dataFimEvento);

            Assert.Throws<EventoParoquialExistenteException>(() => agendaEventoService.Agendar(novoEvento));
        }

        private Evento CriarEvento(TipoEventoEnum tipo, DateTime dataInicio, DateTime dataFim)
        {
            Grupo grupo = new Grupo("Grupo");

            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, tipo);
        }

        private void CriarEventoParoquialPreExistente()
        {
            DateTime dataInicio = DateTime.Now;
            DateTime dataFim = dataInicio.AddHours(4);
            eventoParoquialExistente = CriarEvento(TipoEventoEnum.Paroquial, dataInicio, dataFim);

            DataBaseSession.Store(eventoParoquialExistente);
            DataBaseSession.SaveChanges();
        }
    }
}