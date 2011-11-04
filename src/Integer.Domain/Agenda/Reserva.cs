﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using DbC;
using Integer.Infrastructure.DocumentModelling;

namespace Integer.Domain.Agenda
{
    public class Reserva : IEquatable<Reserva>
    {
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public DenormalizedReference<Local> Local { get; private set; }
        public Horario Horario 
        { 
            get 
            {
                return new Horario(this.DataInicio, this.DataFim);
            }
            private set 
            {
                this.DataInicio = value.Inicio;
                this.DataFim = value.Fim;
            }
        }

        private string NomeLocal 
        {
            get 
            {
                string identificacaoLocal = "";
                if (Local != null && !String.IsNullOrEmpty(Local.Nome))
                    identificacaoLocal = String.Format(" do local {0} ", Local.Nome);

                return identificacaoLocal;
            }
        }

        public Reserva(Local local, DateTime dataInicio, DateTime dataFim)
        {
            PreencherLocal(local);
            PreencherDatas(dataInicio, dataFim);
        }

        private void PreencherLocal(Local local)
        {
            #region pré-condição

            Assertion localNaoEhNulo = Assertion.That(local != null).WhenNot("Local da reserva não pode ser nulo.");

            #endregion
            localNaoEhNulo.Validate();

            this.Local = local;
        }

        private void PreencherDatas(DateTime dataInicio, DateTime dataFim)
        {
            #region pré-condição

            Assertion dataInicioEhValida = Assertion.That(dataInicio != default(DateTime)).WhenNot(String.Format("Data Início da reserva {0} precisa ser informada.", NomeLocal));
            Assertion dataFimEhValida = Assertion.That(dataFim != default(DateTime)).WhenNot(String.Format("Data Fim da reserva {0} precisa ser informada.", NomeLocal));
            Assertion dataInicioEhAnteriorADataFim = Assertion.That(dataInicio < dataFim).WhenNot(String.Format("Data Fim da reserva {0} deve ser posterior à Data Início.", NomeLocal));

            #endregion
            ((dataInicioEhValida & dataFimEhValida) & dataInicioEhAnteriorADataFim).Validate();

            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
        }

        public void AlterarHorario(Horario novoHorario)
        {
            this.Horario = novoHorario;
        }

        public bool PossuiConflitoCom(Reserva reserva) 
        {
            var dataInicioOutraReserva = reserva.DataInicio.Subtract(Horario.INTERVALO_MINIMO_ENTRE_EVENTOS_E_RESERVAS);
            var dataFimOutraReserva = reserva.DataFim.AddMinutes(Horario.INTERVALO_MINIMO_ENTRE_EVENTOS_E_RESERVAS.Minutes);

            return this.Local.Equals(reserva.Local)
                                    && ((this.DataInicio <= dataInicioOutraReserva && dataInicioOutraReserva <= this.DataFim)
                                        || (this.DataInicio <= dataFimOutraReserva && dataFimOutraReserva <= this.DataFim)
                                        || (dataInicioOutraReserva <= this.DataInicio && this.DataFim <= dataFimOutraReserva));
        }

        public bool Equals(Reserva outraReserva)
        {
            if (outraReserva == null)
                return false;

            return (this.Local.Equals(outraReserva.Local)
                    && this.DataInicio.Equals(outraReserva.DataInicio)
                    && this.DataFim.Equals(outraReserva.DataFim));
        }

        public override bool Equals(object obj)
        {
            Reserva outraReserva = obj as Reserva;
            if (outraReserva == null)
                return false;

            return Equals(outraReserva);
        }

        public override int GetHashCode()
        {
            return (Local.GetHashCode() 
                        ^ DataInicio.GetHashCode()
                        ^ DataFim.GetHashCode());
        }
    }
}
