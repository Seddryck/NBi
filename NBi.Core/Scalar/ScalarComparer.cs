using NBi.Extensibility.Resolving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar
{
    public class ScalarComparer<T> : IComparer<IScalarResolver<T>>
    {

        public int Compare(T? x, T? y)
        {
            return System.Collections.Comparer.DefaultInvariant.Compare(x, y);
        }

        public int Compare(IScalarResolver<T>? x, IScalarResolver<T>? y)
        {
            if (x is null || y is null)
                return -1;
            return Compare(x.Execute(), y.Execute());
        }

        public int Compare(object? x, object? y)
        {
            if (x is IScalarResolver<T> xResolver) 
                x = xResolver.Execute();

            if (y is IScalarResolver<T> yResolver)
                y = yResolver.Execute();

            if (x is null && y is null)
                return 0;
            if (x is null && y is not null)
                return -1;
            if (x is not null && y is null)
                return 1;

            return Compare((T)x!, (T)y!);
        }
    }
}
