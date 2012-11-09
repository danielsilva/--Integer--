using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Domain.Paroquia;
using DbC;
using Integer.Infrastructure.DocumentModelling;

namespace Integer.Domain.Agenda
{
    [Serializable]
    public class Reserva : IEquatable<Reserva>
    {
        public DateTime Data { get; set; }
        public IList<HoraReservaEnum> Hora { get; set; }
        public DenormalizedReference<Local> Local { get; private set; }

        private Reserva() { }

        public Reserva(DenormalizedReference<Local> local, DateTime data, IList<HoraReservaEnum> hora)
        {
            PreencherLocal(local);
            Data = data.Date;
            Hora = hora;
        }

        private void PreencherLocal(DenormalizedReference<Local> local)
        {
            #region pré-condição

            Assertion localNaoEhNulo = Assertion.That(local != null).WhenNot("Local da reserva não pode ser nulo.");

            #endregion
            localNaoEhNulo.Validate();

            this.Local = local;
        }

        public void AlterarHorario(DateTime data, IList<HoraReservaEnum> hora)
        {
            this.Data = data;
            this.Hora = hora;
        }

        public bool PossuiConflitoCom(Reserva reserva) 
        {
            return this.Local.Equals(reserva.Local) 
                && (this.Data == reserva.Data) 
                && (this.Hora.Intersect(reserva.Hora).Count() > 0);
        }

        public bool Equals(Reserva outraReserva)
        {
            if (outraReserva == null)
                return false;

            return (this.Local.Equals(outraReserva.Local)
                && this.Data.Date == outraReserva.Data.Date    
                && this.Hora.Intersect(outraReserva.Hora).Count() == this.Hora.Count);
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
                        ^ Hora.GetHashCode());
        }
    }
}
