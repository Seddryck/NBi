using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class Level
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }

        public Level(string uniqueName, string caption)
        {
            UniqueName = uniqueName;
            Caption = caption;
        }

        public Level Clone()
        {
            return new Level(UniqueName, Caption);
        }
    }
}
