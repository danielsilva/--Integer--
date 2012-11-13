using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Domain.Paroquia;
using System.Web.Mvc;
using Foolproof;
using System.ComponentModel.DataAnnotations;
using Integer.Domain.Agenda;

namespace Integer.Api.Models
{
    public class ReservaDeLocalModel
    {
        [Required(ErrorMessage = "obrigatório")]
        public string LocalId { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        public DateTime? Data { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        public IList<HoraReservaEnum> Hora { get; set; }

        public string DataUtc 
        {
            get 
            {
                if (Data.HasValue)
                    return Data.Value.ToUniversalTime().ToString();
                else
                    return "";
            }
        }
    }
}