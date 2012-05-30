using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public interface IStructure
    {
        string Caption {get; set;}
        string UniqueName {get;}
    }
}
