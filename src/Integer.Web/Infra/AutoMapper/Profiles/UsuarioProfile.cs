using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Integer.Domain.Paroquia;
using Integer.Web.ViewModels;

namespace Integer.Web.Infra.AutoMapper.Profiles
{
    public class UsuarioProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UsuarioCriarViewModel, Usuario>()
                .ConstructUsing(m => new Usuario(m.Email, m.Senha));
        }
    }
}