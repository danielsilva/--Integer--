using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using Integer.Domain.Agenda;

namespace Integer.Api.Models
{
    public class EventoModel
    {
        public EventoModel()
        {
            DataCadastro = DateTime.Now;
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [StringLength(50, ErrorMessage = "máximo 50 caracteres")]
        public string Nome { get; set; }

        [StringLength(250, ErrorMessage = "máximo de 250 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [GreaterThan("DataInicio", ErrorMessage = "deve ser posterior ao início")]
        public DateTime? DataFim { get; set; }

        public DateTime? DataCadastro { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        public int Tipo { get; set; }

        [Required(ErrorMessage = "É necessário reservar locais para o evento.")]
        public IList<ReservaDeLocalModel> Reservas { get; set; }
    }
}