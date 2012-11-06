using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Web.ViewModels;
using Integer.Domain.Acesso;

namespace Integer.Web.Infra.AutoMapper.Profiles
{
    public class UsuarioProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UsuarioCriarViewModel, Usuario>()
                .ForMember(x => x.Email, o => o.Ignore())
                .ForMember(x => x.Senha, o => o.Ignore())
                .ConstructUsing(m => new Usuario(m.Email, m.Senha));
        }
    }
}