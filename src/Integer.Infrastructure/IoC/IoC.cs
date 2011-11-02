using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Infrastructure.IoC
{
    public static class IoCWorker
    {
        private static DependencyResolver resolver;

        public static bool IsInitialized 
        {
            get 
            {
                return resolver != null;
            }
        }

        public static void Initialize(DependencyResolver resolver)
        {
            IoCWorker.resolver = resolver;
        }

        public static T Resolve<T>()
        {
            return resolver.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return resolver.Resolve<T>(name);
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return resolver.ResolveAll<T>();
        }
    }
}
