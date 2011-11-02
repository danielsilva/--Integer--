using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;
using Integer.Domain.Agenda;

namespace Integer.Domain.Services
{
    public class RemoveConflitoService : DomainEventHandler<EventoCanceladoEvent>, DomainEventHandler<HorarioDeReservaDeLocalAlteradoEvent>
    {
        private readonly Eventos eventos;

        public RemoveConflitoService(Eventos eventos)
        {
            this.eventos = eventos;
        }

        public void Handle(EventoCanceladoEvent cancelamento)
        {
            Evento eventoCancelado = cancelamento.Evento;
            foreach (Evento evento in eventos.QuePossuemConflitosCom(eventoCancelado))
            {
                evento.RemoverConflitoCom(eventoCancelado);
            }
        }

        public void Handle(HorarioDeReservaDeLocalAlteradoEvent alteracaoDeHorario)
        {
            Evento evento = alteracaoDeHorario.Evento;
            Reserva reservaAlterada = alteracaoDeHorario.Reserva;

            IEnumerable<Evento> eventosComConflito = eventos.QuePossuemConflitoCom(evento, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            foreach (Evento eventoComConflito in eventosComConflito)
            {
                Reserva reservaComConflito = eventoComConflito.Reservas.Single(r => r.Local == reservaAlterada.Local);
                
                //TODO verificar se o conflito foi resolvido. Caso positivo, removê-lo
                //reservaComConflito.
            }
        }
    }
}
