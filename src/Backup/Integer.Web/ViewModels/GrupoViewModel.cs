using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Integer.Web.ValidationHelpers;

namespace Integer.Web.ViewModels
{
    public class GrupoViewModel
    {
        public GrupoViewModel()
        {
            Grupos = new List<ItemViewModel>();
        }
        public bool leaf { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessage = "obrigatório")]        
        public string Nome { get; set; }

        [Display(Name="E-mail", Prompt="E-mail cadastrado no portal da paróquia ([grupo]@paroquiadivinosalvador.com.br)")]
        [Required(ErrorMessage = "obrigatório")]
        [CustomValidation(typeof(ValidationHelper), "ValidarEmail")]
        public string Email { get; set; }

        [Display(Name = "Cor do evento")]
        public string CorNoCalendario { get; set; }

        [Display(Name = "Grupo Pai")]
        public string GrupoPai { get; set; }

        public IEnumerable<ItemViewModel> Grupos { get; set; }

        public IEnumerable<GrupoViewModel> children { get; set; }
    }
}