using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using DbC;
using System.Threading;
using Integer.Infrastructure.DateAndTime;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_PossuiPrioridadeSobre
    {
        [Fact]
        public void EventoParoquial_PossuiPrioridadeSobre_EventoSacramento() 
        {
            Evento eventoParoquial = CriarEvento(TipoEventoEnum.Paroquial);
            Evento eventoSacramento = CriarEvento(TipoEventoEnum.Sacramento);

            Assert.True(eventoParoquial.PossuiPrioridadeSobre(eventoSacramento));
        }

        [Fact]
        public void EventoSacramento_PossuiPrioridadeSobre_EventoGrandeNumeroDePessoas()
        {
            Evento eventoSacramento = CriarEvento(TipoEventoEnum.Sacramento);
            Evento eventoGrande = CriarEvento(TipoEventoEnum.GrandeMovimentoDePessoas);

            Assert.True(eventoSacramento.PossuiPrioridadeSobre(eventoGrande));
        }

        [Fact]
        public void EventoGrande_PossuiPrioridadeSobre_EventoComum() 
        {
            Evento eventoGrande = CriarEvento(TipoEventoEnum.GrandeMovimentoDePessoas);
            Evento eventoComum = CriarEvento(TipoEventoEnum.Comum);

            Assert.True(eventoGrande.PossuiPrioridadeSobre(eventoComum));
        }

        [Fact]
        public void EventoDeMesmoTipo_CadastradoAntes_PossuiPrioridade() 
        {
            var random = new Random();
            int idTipoEvento = random.Next((int)TipoEventoEnum.Paroquial, (int)TipoEventoEnum.Comum);

            DateTime dataAtual = DateTime.Now;
            
            SystemTime.Now = () => dataAtual;
            Evento primeiroEvento = CriarEvento((TipoEventoEnum)idTipoEvento);
            
            SystemTime.Now = () => dataAtual.AddMinutes(1);
            Evento segundoEvento = CriarEvento((TipoEventoEnum)idTipoEvento);

            Assert.True(primeiroEvento.PossuiPrioridadeSobre(segundoEvento));
        }

        private Evento CriarEvento(TipoEventoEnum tipoEvento)
        {
            var grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", "Descricao", DateTime.Now, DateTime.Now.AddHours(1), grupo, tipoEvento);
        }
    }
}