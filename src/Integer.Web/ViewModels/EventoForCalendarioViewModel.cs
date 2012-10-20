using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Integer.Web.ViewModels
{
    public class EventoForCalendarioViewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string group { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string notes { get; set; }
    }
}