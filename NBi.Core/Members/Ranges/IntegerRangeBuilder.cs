using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Members.Ranges
{
    internal class IntegerRangeBuilder : BaseBuilder
    {
        protected new IIntegerRange Range
            =>(IIntegerRange)(base.Range ?? throw new NullReferenceException());

        protected override void InternalBuild()
        {
            Result = Build(Range.Start, Range.End, Range.Step);
        }

        public IEnumerable<string> Build(int start, int end, int step)
        {
            var list = new List<string>();
            for (int i = start; i <= end; i+=step)
                list.Add(i.ToString());

            return list;
        }
    }
}
