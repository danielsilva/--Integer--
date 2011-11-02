using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Infrastructure.Events
{
    public interface DomainEventHandler<T> where T : DomainEvent
    {
        void Handle(T args);
    }
}
