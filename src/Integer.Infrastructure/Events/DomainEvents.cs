using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Integer.Infrastructure.IoC;

namespace Integer.Infrastructure.Events
{
    public static class DomainEvents
    {
        [ThreadStatic] //cada thread contém seus callbacks
        private static List<Delegate> actions;

        //Registra um callback para o domain event
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
                actions = new List<Delegate>();

            actions.Add(callback);
        }

        //Clears callbacks passed to Register on the current thread
        public static void ClearCallbacks()
        {
            actions = null;
        }

        //Raises the given domain event
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (IoCWorker.IsInitialized)
            {
                var handlers = IoCWorker.ResolveAll<IDomainEventHandler<T>>();
                foreach (var handler in handlers)
                {
                    handler.Handle(args);
                }
            }

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }
    }
}
