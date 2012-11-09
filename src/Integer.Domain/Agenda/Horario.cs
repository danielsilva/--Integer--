using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbC;

namespace Integer.Domain.Agenda
{
    public class Horario : IEquatable<Horario>
    {
        public static readonly TimeSpan INTERVALO_MINIMO_ENTRE_EVENTOS_E_RESERVAS = TimeSpan.FromMinutes(59);

        private readonly DateTime dataInicio;
        private readonly DateTime dataFim;

        private Horario() { }

        public Horario(DateTime dataInicio, DateTime dataFim)
        {
            var dataInicioEhMenorQueDataFim = Assertion.That(dataInicio < dataFim).WhenNot("Data Início deve ser anterior à Data Fim.");
            dataInicioEhMenorQueDataFim.Validate();

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

        public DateTime Inicio 
        { 
            get 
            {
                return this.dataInicio;
            } 
        }

        public DateTime Fim 
        { 
            get 
            {
                return this.dataFim;
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

        public bool VerificarConcorrencia(Horario outroHorario)
        {
            bool esteHorarioTerminaAntes = this.dataFim < outroHorario.dataInicio;
            bool esteHorarioComecaDepois = outroHorario.dataFim < this.dataInicio;

            return !(esteHorarioTerminaAntes || esteHorarioComecaDepois);
        }

        public bool Equals(Horario outroHorario)
        {
            if (outroHorario == null)
                return false;

            return (this.dataInicio.Equals(outroHorario.dataInicio)
                    && this.dataFim.Equals(outroHorario.dataFim));
        }

        public override bool Equals(object obj)
        {
            Horario outroHorario = obj as Horario;
            if (outroHorario == null)
                return false;

            return Equals(outroHorario);
        }

        public override int GetHashCode()
        {
            return dataInicio.GetHashCode() ^ dataFim.GetHashCode();
        }
    }
}
