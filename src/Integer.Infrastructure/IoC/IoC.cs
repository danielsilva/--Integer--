using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Dependencies;


namespace Integer.Infrastructure.IoC
{
    public static class IoCWorker
    {
        private static IDependencyResolver resolver;

        public static bool IsInitialized 
        {
            get 
            {
                return resolver != null;
            }
        }

        public static void Initialize(IDependencyResolver resolver)
        {
            IoCWorker.resolver = resolver;
        }

        public static T Resolve<T>()
        {
            return (T)resolver.GetService(typeof(T));
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)resolver.GetServices(typeof(T));
        }
    }
}
