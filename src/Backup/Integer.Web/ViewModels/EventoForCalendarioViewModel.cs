using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Integer.Web.ViewModels
{
    public class EventoForCalendarioViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string GrupoId { get; set; }
        public string Grupo { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public string Locais { get; set; }
        public string TipoId { get; set; }
        public string AgendaId { get; set; }
        public IEnumerable<ReservaDeLocalViewModel> Reservas { get; set; }
    }
}