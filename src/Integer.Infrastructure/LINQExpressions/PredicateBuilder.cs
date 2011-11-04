using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Integer.Infrastructure.LINQExpressions
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> InitializeWithTrue<T>() { return f => true; }
        public static Expression<Func<T, bool>> InitializeWithFalse<T>() { return f => false; }
    }
}
