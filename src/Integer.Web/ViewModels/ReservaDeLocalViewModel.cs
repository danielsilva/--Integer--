using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Domain.Paroquia;
using System.Web.Mvc;
using Foolproof;
using System.ComponentModel.DataAnnotations;

namespace Integer.Web.ViewModels
{
    public class ReservaDeLocalViewModel
    {
        [Required(ErrorMessage = "obrigatório")]
        public string LocalId { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        public DateTime? Data { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        public IEnumerable<HoraReservaEnum> Hora { get; set; }
    }
}