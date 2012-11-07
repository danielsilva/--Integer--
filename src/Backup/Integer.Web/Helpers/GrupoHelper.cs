using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Web.ViewModels;
using Integer.Domain.Paroquia;
using System.Text;

namespace Integer.Web.Helpers
{
    public class GrupoHelper
    {
        private Grupos grupos;
        private IEnumerable<Grupo> gruposExistentes;

        public GrupoHelper(Grupos grupos)
        {
            this.grupos = grupos;
        }

        List<GrupoViewModel> gruposOrganizados;
        short nivelArvoreGrupos = 0;
        public List<GrupoViewModel> CriarListaGrupos()
        {
            gruposExistentes = grupos.Todos().OrderBy(g => g.Nome).ToList();

            // TODO organizar grupos por grupos do conselho
            gruposOrganizados = new List<GrupoViewModel>();
            foreach (var grupo in gruposExistentes)
            {
                gruposOrganizados.Add(new GrupoViewModel() { Id = grupo.Id, Nome = grupo.Nome });
            }

            return gruposOrganizados;            
            /*IEnumerable<string> idsDeGruposQueSaoFilhos = gruposExistentes.SelectMany(g => g.GruposFilhos.Select(filho => filho.Id));
            Grupo grupoRaiz = gruposExistentes.Where(g => !idsDeGruposQueSaoFilhos.Contains(g.Id)).SingleOrDefault();
            
            if (grupoRaiz != null)
            {
                gruposOrganizados = new List<GrupoViewModel>();
                gruposOrganizados.Add(new GrupoViewModel() { Id = grupoRaiz.Id, Nome = grupoRaiz.Nome });
                AdicionarGruposFilhos(grupoRaiz);
            }

            return gruposOrganizados;*/
        }

        private void AdicionarGruposFilhos(Grupo grupoPai)
        {
            if (grupoPai.GruposFilhos != null)
            {
                nivelArvoreGrupos++;
                foreach (var grupoFilho in grupoPai.GruposFilhos.OrderBy(g => g.Nome))
                {
                    GrupoViewModel grupoDTO = new GrupoViewModel() { Id = grupoFilho.Id, Nome = grupoFilho.Nome };
                    if (!gruposOrganizados.Contains(grupoDTO))
                    {
                        grupoDTO.Nome = grupoFilho.Nome;
                        for (int i = nivelArvoreGrupos; i > 0; i--) { grupoDTO.Nome = grupoDTO.Nome.Insert(0, "---"); };
                        gruposOrganizados.Add(grupoDTO);
                    }
                    
                    AdicionarGruposFilhos(gruposExistentes.Single(g => g.Id == grupoFilho.Id));
                }
                nivelArvoreGrupos--;
            }
        }
    }
}