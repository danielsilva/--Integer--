using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Infrastructure.DocumentModelling
{
    public interface INamedDocument
    {
        string Id { get; set; }
        string Nome { get; set; }
    }
}
