using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Comparer
{
    public class TextTolerance : Tolerance
    {
        protected TextTolerance(string value)
            : base(value)
        { }

        public static TextTolerance None
        {
            get
            {
                none ??= new TextTolerance("None");
                return none;
            }
        }

        private static TextTolerance? none;
    }
}
