using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class Property : IStructure
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }

        public Property(string uniqueName, string caption)
        {
            UniqueName = uniqueName;
            Caption = caption;
        }

        public Property Clone()
        {
            return new Property(UniqueName, Caption);
        }
    }
}
