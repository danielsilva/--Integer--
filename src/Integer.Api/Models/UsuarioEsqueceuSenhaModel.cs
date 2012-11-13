using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using Integer.Api.Infra;

namespace Integer.Api.Models
{
    public class UsuarioEsqueceuSenhaModel
    {
        [Required(ErrorMessage="obrigatório")]
        public string Email { get; set; }
    }
}