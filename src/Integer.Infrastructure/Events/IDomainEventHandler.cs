using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Infrastructure.Events
{
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}
