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

        public int Compare(T x, T y)
        {
            return System.Collections.Comparer.DefaultInvariant.Compare(x, y);
        }

        public int Compare(IScalarResolver<T> x, IScalarResolver<T> y)
        {
            return Compare(x.Execute(), y.Execute());
        }

        public int Compare(object x, object y)
        {
            if (x is IScalarResolver<T>) 
                x = ((IScalarResolver<T>)x).Execute();

            if (y is IScalarResolver<T>)
                y = ((IScalarResolver<T>)y).Execute();

            return Compare((T)x, (T)y);
        }
    }
}
