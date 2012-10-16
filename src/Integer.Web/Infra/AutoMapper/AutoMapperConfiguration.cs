using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Web.Infra.AutoMapper.Profiles;
using AutoMapper;

namespace Integer.Web.Infra.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.AddProfile(new GrupoProfile());
            Mapper.AddProfile(new UsuarioProfile());
            Mapper.AddProfile(new LocalProfile());
            Mapper.AddProfile(new EventoProfile());
        }
    }
}