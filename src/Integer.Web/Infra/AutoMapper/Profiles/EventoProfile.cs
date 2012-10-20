using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Web.ViewModels;
using Integer.Domain.Agenda;

namespace Integer.Web.Infra.AutoMapper.Profiles
{
    public class EventoProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Evento, EventoForCalendarioViewModel>()
                .ForMember(x => x.id, o => o.MapFrom(m => m.Id))
                .ForMember(x => x.title, o => o.MapFrom(m => m.Nome))
                .ForMember(x => x.description, o => o.MapFrom(m => m.Descricao))
                .ForMember(x => x.group, o => o.MapFrom(m => m.Grupo.Nome))
                .ForMember(x => x.start, o => o.MapFrom(m => m.DataInicio.ToUniversalTime().ToString("o")))
                .ForMember(x => x.end, o => o.MapFrom(m => m.DataFim.ToUniversalTime().ToString("o")));
        }
    }
}