using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Agenda;
using Integer.Domain.Paroquia;
using Rhino.Mocks;
using DbC;
using Xunit;

namespace Integer.UnitTests.Domain.Agenda
{
    public class Evento_ReservarLocal
    {
        [Fact]
        public void AoReservarComSucesso_ReservaEstahAssociadoAEvento() 
        {
            Local local = MockRepository.GenerateStub<Local>();

            DateTime dataInicioEvento = DateTime.Now;
            DateTime dataFimEvento = dataInicioEvento.AddHours(4);
            Evento evento = CriarEvento(dataInicioEvento, dataFimEvento);
            evento.Reservar(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Manha });

            Assert.Equal(1, evento.Reservas.Count());
            Reserva reservaEsperada = new Reserva(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Manha });
            Assert.Equal(reservaEsperada, evento.Reservas.First());
        }

        [Fact]
        public void QuandoJaExisteReservaDeUmLocalParaOutroHorarioParecido_DisparaExcecao() 
        {
            Local local = MockRepository.GenerateStub<Local>();

            DateTime dataInicioEvento = DateTime.Now;
            DateTime dataFimEvento = dataInicioEvento.AddHours(4);
            Evento evento = CriarEvento(dataInicioEvento, dataFimEvento);

            evento.Reservar(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Manha });
            Assert.Throws<DbCException>(() => evento.Reservar(local, dataInicioEvento.Date, new List<HoraReservaEnum> { HoraReservaEnum.Manha }));
        }

        private Evento CriarEvento(DateTime dataInicio, DateTime dataFim)
        {
            var grupo = MockRepository.GenerateStub<Grupo>();
            return new Evento("Nome", "Descricao", dataInicio, dataFim, grupo, TipoEventoEnum.Comum);
        }
    }
}
