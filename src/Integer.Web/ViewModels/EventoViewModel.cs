using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using Integer.Domain.Agenda;

namespace Integer.Web.ViewModels
{
    public class EventoViewModel
    {
        //private IGrupoRepository grupoRepository;
        //public IGrupoRepository GrupoRepository 
        //{
        //    private get 
        //    {
        //        if (grupoRepository == null) 
        //        {
        //            grupoRepository = IoCWorker.Resolve<IGrupoRepository>();
        //        }
        //        return grupoRepository;
        //    }
        //    set 
        //    {
        //        grupoRepository = value;
        //    }
        //}

        //private IPublicoAlvoRepository publicoAlvoRepository;
        //public IPublicoAlvoRepository PublicoAlvoRepository
        //{
        //    private get
        //    {
        //        if (publicoAlvoRepository == null)
        //        {
        //            publicoAlvoRepository = IoCWorker.Resolve<IPublicoAlvoRepository>();
        //        }
        //        return publicoAlvoRepository;
        //    }
        //    set
        //    {
        //        publicoAlvoRepository = value;
        //    }
        //}

        //public EventoFormViewModel()
        //    : this(null, null, null, null)
        //{

        //}

        //public EventoFormViewModel(string nome, DateTime? dataInicio, DateTime? dataFim, Usuario usuario)
        //{
        //    this.Nome = nome;
        //    this.DataInicio = dataInicio;
        //    this.DataFim = dataFim;

        //    if (UsuarioLogado != null)
        //    {
        //        this.Grupos = new List<GrupoViewModel>();
        //        var grupos = UsuarioLogado.ObterGruposEmQueAtua();
        //        foreach (var grupo in grupos)
        //        {
        //            Grupos.Add(new GrupoViewModel() { Id = grupo.Id, Nome = grupo.Nome });
        //        }
        //    }

        //    TiposDeEvento = new List<ItemViewModel>();
        //    TiposDeEvento.Bind(typeof(TipoEventoEnum));
        //    if (usuario != null && !usuario.PertenceAoConselho()) 
        //    {
        //        var tipoParoquial = TiposDeEvento.SingleOrDefault(t => t.Id == ((int)TipoEventoEnum.Paroquial).ToString());
        //        TiposDeEvento.Remove(tipoParoquial);
        //    }

        //    // TODO exibir conflitos
        //    //this.ConflitosComEventosMaisPrioritarios = new List<ConflitoViewModel>();
        //    //this.ConflitosComEventosMenosPrioritarios = new List<ConflitoViewModel>();
        //}


        public long Id { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [StringLength(50, ErrorMessage = "máximo 50 caracteres")]
        public string Nome { get; set; }

        [StringLength(250, ErrorMessage = "máximo de 250 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        public DateTime? DataInicio { get; set; }

        [Required(ErrorMessage = "obrigatório")]
        [GreaterThan("DataInicio", ErrorMessage = "deve ser posterior à data inicial.")]
        public DateTime? DataFim { get; set; }

        public DateTime? DataCadastro { get; set; }

        [Required(ErrorMessage = "Seleção obrigatória.")]
        public int Grupo { get; set; }

        [Required(ErrorMessage = "Seleção obrigatória.")]
        public int Tipo { get; set; }

        // TODO exibir conflitos
        //public IList<ConflitoViewModel> ConflitosComEventosMaisPrioritarios { get; set; }
        //public IList<ConflitoViewModel> ConflitosComEventosMenosPrioritarios { get; set; }

        [Required(ErrorMessage = "É necessário reservar locais para o evento.")]
        public IList<ReservaDeLocalViewModel> Reservas { get; set; }

        public string NomeGrupo { get; set; }

        public IList<GrupoViewModel> Grupos { get; set;}
        public IList<ItemViewModel> TiposDeEvento { get; set; }
        public bool podeAlterarConflitos { get; set; }
    }
}