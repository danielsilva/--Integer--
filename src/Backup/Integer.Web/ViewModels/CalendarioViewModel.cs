using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Integer.Web.ViewModels
{
    public class CalendarioViewModel 
    {
        public CalendarioViewModel()
        {
            this.events = new List<IEnumerable<string>>();  
        }

        public List<IEnumerable<string>> events { get; set; }
        public bool issort { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string error { get; set; }
    }

    public class EventoCalendarioViewModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int IsAllDayEvent { get; set; }
        public string Color { get; set; }
        public string RecurringRule { get; set;}

        public IEnumerable<string> GetValues()
        {
            yield return Id.ToString();
            yield return Subject.ToString();
            yield return StartTime.ToString();
            yield return EndTime.ToString();
            yield return Location.ToString();
            yield return Description;
            yield return IsAllDayEvent.ToString();
            yield return Color.ToString();
            yield return RecurringRule.ToString();
            yield return Location.ToString();
        }
    }
}