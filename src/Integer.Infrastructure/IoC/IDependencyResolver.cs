using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Infrastructure.IoC
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
        T Resolve<T>(string name);
        IEnumerable<T> ResolveAll<T>();
    }
}
