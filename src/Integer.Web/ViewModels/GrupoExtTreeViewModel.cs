using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Integer.Web.ValidationHelpers;

namespace Integer.Web.ViewModels
{
    public class GrupoExtTreeViewModel
    {
        public GrupoExtTreeViewModel()
        {
            children = new List<GrupoExtTreeViewModel>();
        }
        
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cor { get; set; }

        public IEnumerable<GrupoExtTreeViewModel> children { get; set; }
        public bool leaf 
        { 
            get 
            {
                return children.Count() == 0;
            } 
        }
    }
}