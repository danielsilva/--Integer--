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
            ViewBag.Tipos = RavenSession.Query<TipoEvento>().OrderBy(t => t.Nome).ToList();

            return View("Calendario");
        }

        [HttpGet]
        public JsonResult Eventos() 
        {
            return Json(new List<dynamic>(){
                new
                {
                    id = 1001,
                    cid = 1,
                    title = "Vacation",
                    start = DateTime.UtcNow.ToString("o"),
                    end = DateTime.UtcNow.AddHours(1).ToString("o"),
                    notes = "Have fun"
                },
                new
                {
                    id = 1002,
                    cid = 1,
                    title = "Vacation",
                    start = DateTime.UtcNow.ToString("o"),
                    end = DateTime.UtcNow.AddDays(20).ToString("o"),
                    notes = "Have fun"
                },
                new
                {
                    id = 1003,
                    cid = 1,
                    title = "Vacation",
                    start = DateTime.UtcNow.ToString("o"),
                    end = DateTime.UtcNow.AddHours(1).ToString("o"),
                    notes = "Have fun"
                },
                new
                {
                    id = 1004,
                    cid = 1,
                    title = "Vacation",
                    start = DateTime.UtcNow.ToString("o"),
                    end = DateTime.UtcNow.AddHours(1).ToString("o"),
                    notes = "Have fun"
                },
                new
                {
                    id = 1005,
                    cid = 1,
                    title = "Vacation",
                    start = DateTime.UtcNow.ToString("o"),
                    end = DateTime.UtcNow.AddHours(1).ToString("o"),
                    notes = "Have fun"
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObterEventos(DateTime showDate, VisualizacaoCalendarioEnum viewType, string idGrupo)
        {
            IEnumerable<Evento> eventos = FiltrarEventos(showDate, viewType, idGrupo);
            CalendarioViewModel calendario = MontarCalendario(eventos);

            return Json(calendario, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<Evento> FiltrarEventos(DateTime dataReferencia, VisualizacaoCalendarioEnum tipoVisualizacao, string idGrupo)
        {
            IEnumerable<Evento> eventosExistentes;

            switch (tipoVisualizacao)
            {
                case VisualizacaoCalendarioEnum.day:
                    if (UsuarioEstaLogado)
                        eventosExistentes = eventos.ObterEventosAgendadosDoDia(dataReferencia, idGrupo);
                    else
                        eventosExistentes = eventos.ObterTodosEventosDoDia(dataReferencia, idGrupo);
                    
                    break;

                case VisualizacaoCalendarioEnum.week:
                    var primeiroDiaDaSemana = dataReferencia.AddDays((int)DayOfWeek.Sunday - (int)dataReferencia.DayOfWeek);
                    var ultimoDiaDaSemana = dataReferencia.AddDays(7 - ((int)dataReferencia.DayOfWeek + 1));
                    
                    if (UsuarioEstaLogado)
                        eventosExistentes = eventos.ObterTodosEventosDaSemana(primeiroDiaDaSemana, ultimoDiaDaSemana, idGrupo);
                    else
                        eventosExistentes = eventos.ObterEventosAgendadosDaSemana(primeiroDiaDaSemana, ultimoDiaDaSemana, idGrupo);

                    break;

                case VisualizacaoCalendarioEnum.month:
                    if (UsuarioEstaLogado)
                        eventosExistentes = eventos.ObterTodosEventosDoMes(dataReferencia, idGrupo);
                    else
                        eventosExistentes = eventos.ObterEventosAgendadosDoMes(dataReferencia, idGrupo);
                    break;

                default:
                    throw new NotImplementedException(String.Format("Tipo de visualização não implementado: {0}.", tipoVisualizacao));
            }

            return eventosExistentes;
        }

        private CalendarioViewModel MontarCalendario(IEnumerable<Evento> eventos)
        {
            CalendarioViewModel calendario;
            if (eventos != null)
                calendario = CalendarioHelper.ContruirCalendario(eventos);
            else
                calendario = new CalendarioViewModel();

            return calendario;
        }

        [HttpPost]
        public void Salvar(EventoViewModel input) 
        {            
            //TODO: parei na hora de pegar o grupo logado

            var evento = RavenSession.Load<Evento>(input.Id) 
                ?? new Evento(input.Nome, input.Descricao, input.DataInicio.GetValueOrDefault(), input.DataFim.GetValueOrDefault(), GrupoLogado, ((TipoEventoEnum)input.Tipo));
            agenda.Agendar(evento);
        }
    }
}