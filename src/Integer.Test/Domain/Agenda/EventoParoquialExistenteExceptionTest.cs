using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Integer.Domain.Paroquia;
using Integer.Domain.Agenda;
using Rhino.Mocks;

namespace Integer.UnitTests.Domain.Agenda
{
    [TestFixture]
    public class EventoParoquialExistenteExceptionTest
    {
        [Test]
        public void ExibeAMensagemCorreta() 
        {
            var dataInicio = new DateTime(2011, 02, 01, 8, 0, 0);
            var dataFim = dataInicio.AddHours(1);
            var horario = new Horario(dataInicio, dataFim);
            
            var mockEvento = MockRepository.GenerateMock<Evento>();
            mockEvento.Expect(e => e.Nome)
                  .Return("Nome do evento");
            mockEvento.Expect(e => e.Horario)
                  .Return(horario);

            var excecao = new EventoParoquialExistenteException(new List<Evento> () { mockEvento });

            var mensagemEsperada = "O evento paroquial 'Nome do evento' já está cadastrado para o horário: 01/02/2011 08:00 até 09:00." + Environment.NewLine;
            Assert.AreEqual(mensagemEsperada, excecao.Message);
        }
    }
}
