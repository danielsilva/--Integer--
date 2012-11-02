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
using Integer.Infrastructure.Email;

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
            var grupos = RavenSession.Query<Grupo>().ToList();
            var emails = new System.Text.StringBuilder();
            foreach (var grupo in grupos)
            {
                emails.Append(", " + grupo.Email);
                //EnviarEmailAvisandoSenha(grupo);
            }


            IEnumerable<Evento> eventos;
            if (GrupoLogado == null)
                eventos = RavenSession.ObterEventosAgendados(startDate, endDate);
            else
                eventos = RavenSession.ObterEventos(startDate, endDate);

            return Json(new { Eventos = eventos.MapTo<EventoForCalendarioViewModel>() }, JsonRequestBehavior.AllowGet);
        }

        private void EnviarEmailAvisandoSenha(Grupo grupo)
        {
            if (grupo.Email == "conselho@paroquiadivinosalvador.com.br"
                || grupo.Email == "sem email")
                return;

            var mensagem = new System.Text.StringBuilder();
            mensagem.Append("<table><tr><td>Para agendar os eventos de 2013, acesse o calendário paroquial utilizando os seguintes dados:</td></tr>");
            mensagem.Append("<tr><td></td></tr>");
            mensagem.Append("<tr><td>E-mail: " + grupo.Email + "</td></tr>");
            mensagem.Append("<tr><td>Senha: " + grupo.SenhaDescriptografada + "</td></tr></table>");

            EmailWrapper.EnviarEmail(grupo.Email, "Calendário Paroquial 2013", mensagem.ToString());
            EmailWrapper.EnviarEmail("danielsilva.rj@gmail.com", "Calendário Paroquial 2013", mensagem.ToString());
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