using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funta.Core.Helper.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }
}
