using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataNotFoundException : Exception
    {
        public MetadataNotFoundException(string format, string name) : base(string.Format(format, name))
        {

        }
    }
}
