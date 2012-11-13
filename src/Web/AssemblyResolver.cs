using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Dispatcher;

namespace Integer.Web
{
    public class AssemblyResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var controllersAssembly = Assembly.LoadFrom(@"C:\dev\Projetos\Integer\src\Integer.Api\bin\Integer.Api.dll");
            baseAssemblies.Add(controllersAssembly);
            return baseAssemblies;
        }
    }
}