using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Domain.Agenda;
using System.Text;
using Integer.Api.Models;

namespace Integer.Api.Infra.AutoMapper.Profiles
{
    public class EventoProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Evento, EventoForCalendarioModel>()
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

        private IEnumerable<ReservaDeLocalModel> ExtrairReservas(IEnumerable<Reserva> reservas)
        {
            var reservasModel = new List<ReservaDeLocalModel>();
            foreach (var reserva in reservas)
            {
                reservasModel.Add(new ReservaDeLocalModel { 
                    LocalId = reserva.Local.Id,
                    Data = reserva.Data,
                    Hora = reserva.Hora
                });
            }
            return reservasModel;
        }
    }
}