using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal abstract class PostCommandFilter
    {
        public abstract bool Evaluate(object row);
        public string Caption { get; set; }
    }
}
