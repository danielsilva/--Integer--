using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Domain.Paroquia;
using Integer.Api.Models;

namespace Integer.Api.Infra.AutoMapper.Profiles
{
    public class GrupoProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Grupo, GrupoModel>();

            Mapper.CreateMap<Grupo, ItemModel>();
        }
    }
}