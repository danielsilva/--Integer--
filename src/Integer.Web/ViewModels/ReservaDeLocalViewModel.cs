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
        private Locais locais;
        private Locais LocaisRepository 
        {
            get
            {
                if (locais == null)
                {
                    locais = DependencyResolver.Current.GetService<Locais>();
                }
                return locais;
            }
        }

        public ReservaDeLocalViewModel() 
            : this(0, null, null)
        {

        }

        public ReservaDeLocalViewModel(short IndiceReserva, DateTime? dataInicio, DateTime? dataFim)
        {
            this.IndiceReserva = IndiceReserva;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;

            this.Locais = new List<ItemViewModel>();
            var locais = LocaisRepository.Todos().OrderBy(l => l.Nome);
            locais.ToList().ForEach(l => Locais.Add(new ItemViewModel() { Id = l.Id, Nome = l.Nome }));
        }

        public short IndiceReserva { get; set; }      

        [Required(ErrorMessage = "obrigatório")]
        public int Local { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [GreaterThan("DataInicio", ErrorMessage = "deve ser posterior à data inicial")]
        public DateTime? DataFim { get; set; }

        public string NomeLocal { get; set; }

        public IList<ItemViewModel> Locais { get; set; }
    }
}