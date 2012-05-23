using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Domain.Agenda;
using Integer.Web.ViewModels;
using Integer.Web.Helpers;
using Integer.Domain.Paroquia;

namespace Integer.Web.Controllers
{
    public class CalendarioController : ControllerBase
    {
        private readonly Eventos eventos;
        private readonly Grupos grupos;

        public CalendarioController(Eventos eventos, Grupos grupos)
        {
            this.eventos = eventos;
            this.grupos = grupos;
        }

        public ActionResult Index()
        {
            return View("Calendario2");
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

        [HttpGet]
        public JsonResult ObterGrupos()
        {
            var listaGrupos = new GrupoHelper(grupos).CriarListaGrupos();
            return Json(listaGrupos, JsonRequestBehavior.AllowGet);
        }
    }
}
