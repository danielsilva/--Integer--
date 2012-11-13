using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using Integer.Api.Infra;

namespace Integer.Api.Models
{
    public class UsuarioCriarSenhaModel
    {
        public string GrupoId { get; set; }

        [Required(ErrorMessage = "Usuário não encontrado")]
        public string UsuarioId { get; set; }

        [Required(ErrorMessage="obrigatório")]
        [EmailAddress(ErrorMessage="e-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [StringLength(8, MinimumLength=6, ErrorMessage="precisa ter entre 6 e 8 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [EqualTo("Senha", ErrorMessage="repita a senha")]
        public string ConfirmacaoSenha { get; set; }
    }
}