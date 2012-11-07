using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using Integer.Web.Infra;

namespace Integer.Web.ViewModels
{
    public class UsuarioCriarViewModel
    {
        [Required(ErrorMessage="É necessário ser um usuário relacionado a um grupo")]
        public string GrupoId { get; set; }

        [Required(ErrorMessage="obrigatório")]
        [Email(ErrorMessage="e-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [StringLength(8, MinimumLength=6, ErrorMessage="precisa ter entre 6 e 8 caracteres")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [EqualTo("Senha", ErrorMessage="repita a senha")]
        public string ConfirmacaoSenha { get; set; }
    }
}