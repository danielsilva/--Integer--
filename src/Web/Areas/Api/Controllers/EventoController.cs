using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Api.Models;
using Integer.Domain.Agenda;
using Integer.Api.Infra.Raven;
using AutoMapper;
using Integer.Api.Infra.AutoMapper;
using Integer.Domain.Agenda.Exceptions;
using System.Net;
using DbC;
using System.Net.Http;
using System.Web.Http;
using Web.Areas.Api.Security;

namespace Web.Areas.Api.Controllers
{
    public class EventoController : BaseController
    {
        private readonly Eventos eventos;
        private readonly AgendaEventoService agenda;        

        public EventoController(Eventos eventos, AgendaEventoService agenda)
        {
            this.eventos = eventos;
            this.agenda = agenda;
        }

        public IEnumerable<EventoForCalendarioModel> Get(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Evento> eventos;
            if (GrupoLogado == null)
                eventos = RavenSession.ObterEventosAgendados(startDate, endDate);
            else
                eventos = RavenSession.ObterEventos(startDate, endDate);

            return eventos.MapTo<EventoForCalendarioModel>();
        }

        public HttpResponseMessage Post(EventoModel input) 
        {
            Evento evento;
            try
            {
                evento = CriarEvento(input);
                agenda.Agendar(evento);

                input.Id = evento.Id;
            }
            catch (Exception ex)
            {
                if (ex is DbCException
                    || ex is LocalReservadoException
                    || ex is EventoParoquialExistenteException)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message.Replace(Environment.NewLine, "<br/>"));
                }
                else
                {
                    throw;
                }
            }
            return Request.CreateResponse<EventoModel>(HttpStatusCode.Created, input);
        }

        private Evento CriarEvento(EventoModel input)
        {
            Evento evento = new Evento(
                input.Nome,
                input.Descricao,
                input.DataInicio.GetValueOrDefault(),
                input.DataFim.GetValueOrDefault(),
                GrupoLogado,
                ((TipoEventoEnum)input.Tipo)
            );
            foreach (var reserva in input.Reservas)
            {
                var local = RavenSession.ObterLocais().First(l => l.Id == reserva.LocalId);
                evento.Reservar(local, reserva.Data.GetValueOrDefault(), reserva.Hora);
            }
            return evento;
        }

        public HttpResponseMessage Put(string id, EventoModel input) 
        {
            Evento evento = RavenSession.Load<Evento>(input.Id);
            try
            {
                MapearEvento(evento, input);
                agenda.Agendar(evento);
            }
            catch (Exception ex)
            {
                DoNotCallSaveChanges = true;
                if (ex is DbCException
                    || ex is LocalReservadoException
                    || ex is EventoParoquialExistenteException)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message.Replace(Environment.NewLine, "<br/>"));
                }
                else
                {
                    throw;
                }
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private Evento MapearEvento(Evento evento, EventoModel input)
        {
            evento.Alterar(input.Nome, input.Descricao, input.DataInicio.GetValueOrDefault(), input.DataFim.GetValueOrDefault(), GrupoLogado, ((TipoEventoEnum)input.Tipo));
            var reservas = new List<Reserva>();
            foreach (var reserva in input.Reservas)
            {
                var local = RavenSession.ObterLocais().First(l => l.Id == reserva.LocalId);
                reservas.Add(new Reserva(local, reserva.Data.GetValueOrDefault(), reserva.Hora));
            }
            evento.AlterarReservasDeLocais(reservas);

            return evento;
        }

        public void Delete(string id)
        {
            var evento = RavenSession.Load<Evento>(id);
            if (evento == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            evento.CancelarAgendamento();
        }

        [System.Web.Mvc.ActionName("Tipos")]
        public IEnumerable<ItemModel> GetTipos() 
        {
            var tipos = RavenSession.ObterTiposDeEvento();
            return tipos.MapTo<ItemModel>();
        }
    }
}
