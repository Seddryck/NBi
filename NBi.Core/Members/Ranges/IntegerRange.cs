using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Members.Ranges
{
    public class IntegerRange : IRange
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Step { get; set; }
    }
}
