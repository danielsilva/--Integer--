using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Web.ViewModels;
using Integer.Domain.Agenda;
using System.Text;

namespace Integer.Web.Helpers
{
    public class CalendarioHelper
    {
        public static CalendarioViewModel ContruirCalendario(IEnumerable<Evento> eventos) 
        {
            List<EventoCalendarioViewModel> listaEventos = new List<EventoCalendarioViewModel>();

            //TODO RETIRAR
            listaEventos.Add(new EventoCalendarioViewModel()
            {
                Id = "1",
                Subject = "Teste",
                Description = "",
                StartTime = "11/5/2011 08:00",
                EndTime = "11/5/2011 10:00",
                Location = "",
                IsAllDayEvent = 0,
                Color = "0",
                RecurringRule = ""
            });

            foreach (Evento evento in eventos)
            {
                listaEventos.Add(new EventoCalendarioViewModel()
                {
                    Id = evento.Id,
                    Subject = evento.Nome,
                    Description = evento.Descricao,
                    StartTime = evento.DataInicio.ToString("M/d/yyyy HH:mm"),
                    EndTime = evento.DataFim.ToString("M/d/yyyy HH:mm"),
                    Location = ObterTextoLocais(evento),
                    IsAllDayEvent = 0,
                    Color = ObterCor(evento),
                    RecurringRule = ""
                });
            }

            var calendario = new CalendarioViewModel();
            foreach (EventoCalendarioViewModel item in listaEventos)
            {
                calendario.events.Add(item.GetValues());
            }

            calendario.start =  eventos.Count() > 0 ? eventos.Min(e => e.DataInicio).ToString("M/d/yyyy HH:mm") : "";
            calendario.end = eventos.Count() > 0 ? eventos.ToList().Max(e => e.DataFim).ToString("M/d/yyyy HH:mm") : "";
            calendario.issort = true;

            return calendario;
        }

        private static string ObterTextoLocais(Evento evento) 
        {
            StringBuilder textoLocais = new StringBuilder();
            string separador = "";
            foreach (var reserva in evento.Reservas)
            {                
                textoLocais.Append(separador + reserva.Local.Nome);
                separador = ", ";
            }
            return textoLocais.ToString();
        }

        private static string ObterCor(Evento evento) 
        {
            // TODO colocar cor
            //if (evento.Estado == EstadoEventoEnum.NaoAgendado) 
            //{
            //    return "8";
            //}
            //if (evento.Tipo == TipoEventoEnum.Paroquial) 
            //{
            //    return "1";
            //}
            //else if (evento.Grupo.Id == "1")
            //{
            //    return "0";
            //}
            //else if (evento.Grupo.Id == "2" || (evento.Grupo.GrupoPai != null && evento.Grupo.GrupoPai.Id == "2")) 
            //{
            //    return "2";
            //}
            //else if (evento.Grupo.Id == "16" || (evento.Grupo.GrupoPai != null && evento.Grupo.GrupoPai.Id == "16"))
            //{
            //    return "3";
            //}
            //else if (evento.Grupo.Id == "28" || (evento.Grupo.GrupoPai != null && evento.Grupo.GrupoPai.Id == "28"))
            //{
            //    return "4";
            //}
            //else if (evento.Grupo.Id == "32" || (evento.Grupo.GrupoPai != null && evento.Grupo.GrupoPai.Id == "32"))
            //{
            //    return "5";
            //}
            //else if (evento.Grupo.Id == "39" || (evento.Grupo.GrupoPai != null && evento.Grupo.GrupoPai.Id == "39"))
            //{
            //    return "6";
            //}
            //else if (evento.Grupo.Id == "49" || (evento.Grupo.GrupoPai != null && evento.Grupo.GrupoPai.Id == "49"))
            //{
            //    return "7";
            //}

            return "0";
        }
    }
}