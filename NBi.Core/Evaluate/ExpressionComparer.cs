using System;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.Evaluate
{
    public class ExpressionComparer
    {
        public Func<Object, Object, ColumnType, string, bool> Compare;

        public bool Equal(Object x, Object y, ColumnType type, string tolerance)
        {
            var comparer = new ComparerFactory().Get(type);
            var res = comparer.Compare(x, y, ToleranceFactory.Build(type, tolerance));
            return res.AreEqual;
        }

        public bool NotEqual(Object x, Object y, ColumnType type, string tolerance)
        {
            return !Equal(x, y, type, tolerance);
        }

    }
}
