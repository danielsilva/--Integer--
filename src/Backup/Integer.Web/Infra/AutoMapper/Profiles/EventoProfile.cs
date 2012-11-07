using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Web.ViewModels;
using Integer.Domain.Agenda;
using System.Text;

namespace Integer.Web.Infra.AutoMapper.Profiles
{
    public class EventoProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Evento, EventoForCalendarioViewModel>()
                .ForMember(x => x.TipoId, o => o.MapFrom(m => (int)m.Tipo))
                .ForMember(x => x.DataInicio, o => o.MapFrom(m => m.DataInicio.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(x => x.DataFim, o => o.MapFrom(m => m.DataFim.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(x => x.Locais, o => o.MapFrom(m => ExtrairLocaisDasReservas(m)))
                .ForMember(x => x.GrupoId, o => o.MapFrom(m => m.Grupo.Id))
                .ForMember(x => x.Grupo, o => o.MapFrom(m => m.Grupo.Nome))
                .ForMember(x => x.Reservas, o => o.MapFrom(m => ExtrairReservas(m.Reservas)))
                .ForMember(x => x.AgendaId, o => o.MapFrom(m => (int)m.Estado));
        }

        private string ExtrairLocaisDasReservas(Evento evento) 
        {
            StringBuilder locais = new StringBuilder();
            foreach (var reserva in evento.Reservas)
            {
                var separador = (locais.Length > 0 ? ", " : "");
                locais.Append(separador + reserva.Local.Nome);
            }
            return locais.ToString();
        }

        private IEnumerable<ReservaDeLocalViewModel> ExtrairReservas(IEnumerable<Reserva> reservas)
        {
            var reservasModel = new List<ReservaDeLocalViewModel>();
            foreach (var reserva in reservas)
            {
                reservasModel.Add(new ReservaDeLocalViewModel { 
                    LocalId = reserva.Local.Id,
                    Data = reserva.Data,
                    Hora = reserva.Hora
                });
            }
            return reservasModel;
        }
    }
}