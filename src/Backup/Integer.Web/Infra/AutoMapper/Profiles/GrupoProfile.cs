using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Domain.Paroquia;
using Integer.Web.ViewModels;

namespace Integer.Web.Infra.AutoMapper.Profiles
{
    public class GrupoProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Grupo, GrupoViewModel>();

            Mapper.CreateMap<Grupo, ItemViewModel>();
        }
    }
}