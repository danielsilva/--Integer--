using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Integer.Api.Models
{
    public class GrupoModel
    {
        public GrupoModel()
        {
            Grupos = new List<ItemModel>();
        }
        public bool leaf { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessage = "obrigatório")]        
        public string Nome { get; set; }

        [Display(Name="E-mail", Prompt="E-mail cadastrado no portal da paróquia ([grupo]@paroquiadivinosalvador.com.br)")]
        [Required(ErrorMessage = "obrigatório")]
        [EmailAddress(ErrorMessage="E-mail inválido")]
        public string Email { get; set; }

        [Display(Name = "Cor do evento")]
        public string CorNoCalendario { get; set; }

        [Display(Name = "Grupo Pai")]
        public string GrupoPai { get; set; }

        public IEnumerable<ItemModel> Grupos { get; set; }

        public IEnumerable<GrupoModel> children { get; set; }
    }
}