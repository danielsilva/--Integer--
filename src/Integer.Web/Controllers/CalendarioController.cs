using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Domain.Agenda;
using Integer.Web.ViewModels;
using Integer.Web.Helpers;
using Integer.Domain.Paroquia;
using Integer.Domain.Services;
using Integer.Web.Infra.AutoMapper;
using DbC;
using System.Net;
using Integer.Web.Infra.Raven;
using Integer.Web.Infra.AutoMapper;

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
        public JsonResult Eventos(DateTime startDate, DateTime endDate) 
        {
            var eventos = RavenSession.ObterEventos(startDate, endDate);
            return Json(new { Eventos = eventos.MapTo<EventoForCalendarioViewModel>() }, JsonRequestBehavior.AllowGet);
        }        

        [HttpPost]
        public JsonResult Salvar(EventoViewModel input) 
        {
            Evento evento;
            try
            {
                evento = GerarEvento(input);
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

        private Evento GerarEvento(EventoViewModel input) 
        {
            Evento evento;

            if (!string.IsNullOrEmpty(input.Id))
            {
                evento = RavenSession.Load<Evento>(input.Id);
                evento.Alterar(input.Nome, input.Descricao, input.DataInicio.GetValueOrDefault(), input.DataFim.GetValueOrDefault(), GrupoLogado, ((TipoEventoEnum)input.Tipo));
                var reservas = new List<Reserva>();
                foreach (var reserva in input.Reservas)
                {
                    var local = RavenSession.ObterLocais().First(l => l.Id == reserva.LocalId);
                    reservas.Add(new Reserva(local, reserva.Data.GetValueOrDefault(), reserva.Hora));
                }
                evento.AlterarReservasDeLocais(reservas);
            }
            else
            {
                evento = new Evento(
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
            }
            return evento;
        }
    }
}