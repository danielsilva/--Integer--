using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Domain.Agenda;
using Integer.Web.ViewModels;
using Integer.Web.Helpers;
using Integer.Domain.Paroquia;
using Integer.Web.Infra.AutoMapper;
using DbC;
using System.Net;
using Integer.Web.Infra.Raven;
using Integer.Infrastructure.Email;
using System.Web.UI;
using Integer.Domain.Agenda.Exceptions;

namespace Integer.Web.Controllers
{
    public class CalendarioController : ControllerBase
    {
        private readonly Eventos eventos;
        private readonly AgendaEventoService agenda;

        public CalendarioController(Eventos eventos, AgendaEventoService agenda)
        {
            this.eventos = eventos;
            this.agenda = agenda;
        }

        public ActionResult Index()
        {
            ViewBag.Tipos = RavenSession.ObterTiposDeEvento();

            return View("Calendario");
        }

        [HttpGet]
        public JsonResult Agendas() {
            return Json(new { Agendas = new List<dynamic>{
                new { id = 1, title = "", desc = "", color = "", hidden = "false" },
                new { id = 2, title = "", desc = "", color = "FF4FDB", hidden = "false" }
            }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Eventos(DateTime startDate, DateTime endDate) 
        {            
            IEnumerable<Evento> eventos;
            if (GrupoLogado == null)
                eventos = RavenSession.ObterEventosAgendados(startDate, endDate);
            else
                eventos = RavenSession.ObterEventos(startDate, endDate);

            return Json(new { Eventos = eventos.MapTo<EventoForCalendarioViewModel>() }, JsonRequestBehavior.AllowGet);
        }       

        [HttpPost]
        [Authorize]
        public JsonResult Salvar(EventoViewModel input) 
        {
            Evento evento;
            try
            {
                evento = CriarEvento(input);
                agenda.Agendar(evento);
            }
            catch (Exception ex)
            {
                if (ex is DbCException
                    || ex is LocalReservadoException
                    || ex is EventoParoquialExistenteException)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { ErrorMessage = ex.Message.Replace(Environment.NewLine, "<br/>") });
                }
                else
                {
                    throw;
                }
            }
            return Json(evento);
        }

        private Evento CriarEvento(EventoViewModel input)
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

        [HttpPost]
        [Authorize]
        public JsonResult Alterar(EventoViewModel input)
        {
            Evento evento = RavenSession.Load<Evento>(input.Id);
            try
            {
                evento.SaveState();
                MapearEvento(evento, input);
                agenda.Agendar(evento);
            }
            catch (Exception ex)
            {
                evento = evento.RestoreState<Evento>();
                if (ex is DbCException
                    || ex is LocalReservadoException
                    || ex is EventoParoquialExistenteException)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { ErrorMessage = ex.Message.Replace(Environment.NewLine, "<br/>") });
                }
                else
                {
                    throw;
                }
            }
            return Json(evento);
        }

        private Evento MapearEvento(Evento evento, EventoViewModel input) 
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

        [HttpPost]
        public void Cancelar(string id)
        {
            var evento = RavenSession.Load<Evento>(id);
            if (evento != null)
                evento.CancelarAgendamento();
        }
    }
}