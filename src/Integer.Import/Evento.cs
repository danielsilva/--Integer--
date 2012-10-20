using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Import
{
    public partial class Evento
    {
        public int Tipo 
        { 
            get 
            {
                return this.idTipo;
            } 
        }

        public int Estado
        {
            get
            {
                return this.idEstado;
            }
        }
    }
}
