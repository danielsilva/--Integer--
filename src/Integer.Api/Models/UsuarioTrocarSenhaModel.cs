using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using Integer.Api.Infra;

namespace Integer.Api.Models
{
    public class UsuarioTrocarSenhaModel
    {
        public string Id { get; set; }
        public Guid Token { get; set; }

        [Required(ErrorMessage="obrigatório")]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "precisa ter entre 6 e 8 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [EqualTo("Senha", ErrorMessage="repita a senha")]
        public string SenhaIgual { get; set; }
    }
}