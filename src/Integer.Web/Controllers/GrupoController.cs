using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Integer.Domain.Paroquia;
using Integer.Web.ViewModels;
using AutoMapper;
using Integer.Infrastructure.Email;

namespace Integer.Web.Controllers
{
    public class GrupoController : ControllerBase
    {
        private Grupos grupos;
        private EmailWrapper Email;

        public GrupoController(Grupos grupos)
        {
            this.grupos = grupos;
        }

        public ActionResult Index()
        {
            IList<ItemViewModel> gruposView = new List<ItemViewModel>();

            IEnumerable<Grupo> gruposExistentes = grupos.Todos();
            foreach (var grupo in gruposExistentes)
            {
                gruposView.Add(Mapper.Map<ItemViewModel>(grupo));
            }

            var grupoViewModel = new GrupoViewModel { 
                Grupos = gruposView
            };

            return View("Grupo", grupoViewModel);
        }

        [HttpGet]
        public ActionResult Criar() 
        {
            return PartialView("GrupoForm", new GrupoViewModel());
        }

        [HttpPost]
        public ActionResult Salvar(GrupoViewModel grupoForm) 
        {
            if (ModelState.IsValid)
            {
                Grupo grupoPai = grupos.Com(g => g.Id == grupoForm.GrupoPai);
                string cor = grupoForm.CorNoCalendario;

                if (String.IsNullOrEmpty(grupoForm.Id))
                {
                    Grupo grupo = new Grupo(grupoForm.Nome, grupoForm.Email, grupoPai, cor);
                    grupos.Salvar(grupo);
                    // TODO enviar email
                    //Email.AgendarEmail(grupo.Email, "{ Integer } Acesso ao calendário", grupo.ObterMensagemBoasVindas());
                }
                else
                {
                    Grupo grupo = grupos.Com(g => g.Id == grupoForm.Id);
                    grupo.Alterar(grupoForm.Nome, grupoForm.Email, grupoPai, cor);
                }
            }
            return PartialView("GrupoForm", grupoForm);
        }

        [HttpGet]
        public JsonResult ObterGrupos() 
        {
            Mapper.CreateMap<Grupo, GrupoExtTreeViewModel>();

            var gruposExistentes = grupos.Todos();

            var gruposExtTree = Mapper.Map<IEnumerable<Grupo>, IEnumerable<GrupoExtTreeViewModel>>(gruposExistentes); 


            return Json(new
            {
                text = ".",
                children = gruposExtTree
            }, JsonRequestBehavior.AllowGet);

            /*

            return @"{
                        'root': {
                                'nome':'Conselho Pastoral Paroquial',
                                'children': [{
                                    nome:'Grupo I (Oração e Espiritualidade)',
                                    email:'---',
                                    cor:' --- ',
                                    children:[{
                                        nome:'Apostolado da Oração',
                                        email:'apostoladodaoracao@paroquiadivinosalvador.com.br',
                                        cor:'---'
                                    }, {
                                        nome:'Intercessão',
                                        email:'intercessao@paroquiadivinosalvador.com.br',
                                        cor:'---'                            
                                }]
                        }
                    }";
             * */
        }
    }
}
