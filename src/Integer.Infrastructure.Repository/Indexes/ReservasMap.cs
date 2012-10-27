using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Integer.Domain.Agenda;
using Raven.Abstractions.Indexing;

namespace Integer.Infrastructure.Repository.Indexes
{
    public class ReservasMap : AbstractIndexCreationTask<Evento, ReservasMap.ReservasResult>
    {
        public class ReservasResult
		{
            public string IdLocal { get; set; }
            public DateTime Data { get; set; }
            public HoraReservaEnum Hora { get; set; }
        }

        public ReservasMap()
        {
            Map = eventos => from evento in eventos
                             from reserva in evento.Reservas
                             from hora in reserva.Hora
                             where evento.Estado != EstadoEventoEnum.Cancelado
                             select new 
                             {
                                 IdLocal = reserva.Local.Id,
                                 Data = reserva.Data,
                                 Hora = hora
                             };

            Store(x => x.IdLocal, FieldStorage.Yes);
            Store(x => x.Data, FieldStorage.Yes);
            Store(x => x.Hora, FieldStorage.Yes);
        }
    }
}
