using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

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
            return resolver.GetService<T>();
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return resolver.GetServices<T>();
        }
    }
}
