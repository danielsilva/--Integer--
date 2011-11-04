using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.Events;
using Integer.Domain.Agenda;

namespace Integer.Domain.Services
{
    public class RemoveConflitoService : DomainEventHandler<EventoCanceladoEvent>, DomainEventHandler<HorarioDeReservaDeLocalAlteradoEvent>, DomainEventHandler<HorarioDeEventoAlteradoEvent>
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
            Evento eventoAlterado = alteracaoDeHorario.Evento;
            IEnumerable<Reserva> reservasAlteradas = alteracaoDeHorario.ReservasAlteradas;

            RemoverConflitosDeReservaDeLocalDeOutrosEventos(eventoAlterado, reservasAlteradas);
            RemoverConflitosDeReservaDeLocalDoEvento(eventoAlterado, reservasAlteradas);            
        }

        private void RemoverConflitosDeReservaDeLocalDeOutrosEventos(Evento eventoAlterado, IEnumerable<Reserva> reservasAlteradas)
        {
            IEnumerable<Evento> eventosComConflito = eventos.QuePossuemConflitoCom(eventoAlterado, MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            foreach (Evento eventoComConflito in eventosComConflito)
            {
                bool possuiConflito = eventoComConflito.VerificarSeReservasPossuemConflito(reservasAlteradas);
                if (!possuiConflito)
                    eventoComConflito.RemoverConflitoCom(eventoAlterado);
            }
        }

        private void RemoverConflitosDeReservaDeLocalDoEvento(Evento eventoAlterado, IEnumerable<Reserva> reservasAlteradas)
        {
            IEnumerable<Conflito> conflitosDeLocal = eventoAlterado.Conflitos.Where(c => c.Motivo == MotivoConflitoEnum.LocalReservadoParaEventoDeMaiorPrioridade);
            if (conflitosDeLocal.Count() > 0) 
            {
                IEnumerable<string> idsEventosConflitantes = conflitosDeLocal.Select(c => c.Evento.Id);
                IEnumerable<Evento> eventosConflitantes = eventos.Todos(e => idsEventosConflitantes.Contains(e.Id));

                foreach (Evento eventoConflitante in eventosConflitantes)
                {
                    bool possuiConflito = eventoConflitante.VerificarSeReservasPossuemConflito(reservasAlteradas);
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

            RemoverConflitosDoEvento(eventoAlterado);               
        }

        private void RemoverConflitosDeEventosNaoParoquiais(Evento eventoAlterado)
        {
            IEnumerable<Evento> eventosComConflito = eventos.QuePossuemConflitoCom(eventoAlterado, MotivoConflitoEnum.ExisteEventoParoquialNaData);
            foreach (Evento evento in eventosComConflito)
            {
                bool possuiConflito = evento.PossuiConflitoDeHorarioCom(eventoAlterado);
                if (!possuiConflito)
                    evento.RemoverConflitoCom(eventoAlterado);
            }
        }

        private void RemoverConflitosDoEvento(Evento eventoAlterado)
        {
            IEnumerable<Conflito> conflitosComEventosParoquiais = eventoAlterado.Conflitos.Where(c => c.Motivo == MotivoConflitoEnum.ExisteEventoParoquialNaData);
            if (conflitosComEventosParoquiais.Count() > 0)
            {
                IEnumerable<string> idsEventosParoquiais = conflitosComEventosParoquiais.Select(c => c.Evento.Id);
                IEnumerable<Evento> eventosParoquiais = eventos.Todos(e => idsEventosParoquiais.Contains(e.Id));

                foreach (Evento evento in eventosParoquiais)
                {
                    bool possuiConflito = evento.PossuiConflitoDeHorarioCom(eventoAlterado);
                    if (!possuiConflito)
                        eventoAlterado.RemoverConflitoCom(evento);
                }
            }
        }
    }
}
