using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Members.Ranges
{
    public class IntegerRange : IRange
    {
        public virtual int Start { get; set; }
        public virtual int End { get; set; }
        public virtual int Step { get; set; }
    }
}
