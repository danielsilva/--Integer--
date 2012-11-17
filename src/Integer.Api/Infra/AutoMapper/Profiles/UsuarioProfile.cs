using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Api.Models;
using Integer.Domain.Acesso;

namespace Integer.Api.Infra.AutoMapper.Profiles
{
    public class UsuarioProfile : Profile
    {
        protected override void Configure()
        {
            //Mapper.CreateMap<UsuarioCriarSenhaModel, Usuario>()
            //    .ForMember(x => x.Email, o => o.Ignore())
            //    .ForMember(x => x.Senha, o => o.Ignore())
            //    .ConstructUsing(m => new Usuario(m.Email, m.Senha, m.GrupoId));
        }
    }
}