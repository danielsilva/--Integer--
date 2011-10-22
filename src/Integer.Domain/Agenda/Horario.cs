using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Domain.Agenda
{
    public class Horario
    {
        private readonly DateTime dataInicio;
        private readonly DateTime dataFim;

        public Horario(DateTime dataInicio, DateTime dataFim)
        {
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
        }

        private string Duracao 
        {
            get
            {
                if (dataInicio.Date == dataFim.Date)
                {
                    return String.Format("{0} até {1}", dataInicio.ToString("dd/MM/yyyy HH:mm"), dataFim.ToString("HH:mm"));
                }
                else
                {
                    return String.Format("{0} até {1}", dataInicio.ToString("dd/MM/yyyy HH:mm"), dataFim.ToString("dd/MM/yyyy HH:mm"));
                }
            }
        }

        public static implicit operator string(Horario horario)
        {
            return horario.Duracao;
        }

        public override string ToString()
        {
            return this.Duracao;
        }
    }
}
