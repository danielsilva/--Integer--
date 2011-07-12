using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integer.Infrastructure.Validation
{
    public class ValidationException : ApplicationException
    {
        public ValidationException()
            : base()
        {
        }

        public ValidationException(string msg)
            : base(msg)
        {
        }
    }
}
