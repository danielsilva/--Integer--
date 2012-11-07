using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using Integer.Web.Infra;

namespace Integer.Web.ViewModels
{
    public class UsuarioTrocarSenhaViewModel
    {
        [Required(ErrorMessage="obrigatório")]
        public string Email { get; set; }
    }
}