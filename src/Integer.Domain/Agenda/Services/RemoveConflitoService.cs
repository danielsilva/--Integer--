using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;
using Integer.Domain.Agenda;

namespace Integer.Domain.Agenda
{
    public class RemoveConflitoService : DomainEventHandler<EventoCanceladoEvent>, 
                                         DomainEventHandler<ReservaDeLocalAlteradaEvent>, 
                                         DomainEventHandler<HorarioDeEventoAlteradoEvent>
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

        public void Handle(ReservaDeLocalAlteradaEvent alteracaoDeReserva)
        {
            Evento eventoAlterado = alteracaoDeReserva.Evento;

            RemoverConflitosDeReservaDeLocalDeOutrosEventos(eventoAlterado);
            RemoverConflitosDeReservaDeLocalDoEvento(eventoAlterado);            
        }

        private void RemoverConflitosDeReservaDeLocalDeOutrosEventos(Evento eventoAlterado)
        {
            IEnumerable<Evento> eventosComConflito = eventos.QuePossuemConflitoCom(eventoAlterado, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            foreach (Evento eventoComConflito in eventosComConflito)
            {
                bool possuiConflito = eventoComConflito.VerificarSeReservasPossuemConflito(eventoAlterado.Reservas);
                if (!possuiConflito)
                    eventoComConflito.RemoverConflitoCom(eventoAlterado);
            }
        }

        private void RemoverConflitosDeReservaDeLocalDoEvento(Evento eventoAlterado)
        {
            IEnumerable<Conflito> conflitosDeLocal = eventoAlterado.Conflitos.Where(c => c.Motivo == MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            if (conflitosDeLocal.Count() > 0) 
            {
                IEnumerable<string> idsEventosConflitantes = conflitosDeLocal.Select(c => c.Evento.Id);
                IEnumerable<Evento> eventosConflitantes = eventos.Com(idsEventosConflitantes);

                foreach (Evento eventoConflitante in eventosConflitantes)
                {
                    bool possuiConflito = eventoConflitante.VerificarSeReservasPossuemConflito(eventoAlterado.Reservas);
                    if (!possuiConflito)
                        eventoAlterado.RemoverConflitoCom(eventoConflitante);
                }
            }
        }

        public void Handle(HorarioDeEventoAlteradoEvent alteracaoDeHorario)
        {
            Evento eventoAlterado = alteracaoDeHorario.EventoAlterado;

            if (eventoAlterado.Tipo == TipoEventoEnum.Paroquial)
                RemoverConflitosDeEventosNaoParoquiais(eventoAlterado);

            RemoverConflitosComEventosParoquiais(eventoAlterado);               
        }

        private void RemoverConflitosDeEventosNaoParoquiais(Evento eventoAlterado)
        {
            IEnumerable<Evento> eventosComConflito = eventos.QuePossuemConflitoCom(eventoAlterado, MotivoConflitoEnum.ExisteEventoParoquialNaData);
            foreach (Evento evento in eventosComConflito)
            {
                bool aindaPossuiConflito = evento.PossuiConflitoDeHorarioCom(eventoAlterado);
                if (!aindaPossuiConflito)
                    evento.RemoverConflitoCom(eventoAlterado);
            }
        }

        private void RemoverConflitosComEventosParoquiais(Evento eventoAlterado)
        {
            IEnumerable<Conflito> conflitosComEventosParoquiais = eventoAlterado.Conflitos.Where(c => c.Motivo == MotivoConflitoEnum.ExisteEventoParoquialNaData);
            if (conflitosComEventosParoquiais.Count() > 0)
            {
                IEnumerable<string> idsEventosParoquiais = conflitosComEventosParoquiais.Select(c => c.Evento.Id);
                IEnumerable<Evento> eventosParoquiais = eventos.Com(idsEventosParoquiais);

                foreach (Evento evento in eventosParoquiais)
                {
                    bool aindaPossuiConflito = evento.PossuiConflitoDeHorarioCom(eventoAlterado);
                    if (!aindaPossuiConflito)
                        eventoAlterado.RemoverConflitoCom(evento);
                }
            }
        }
    }
}
