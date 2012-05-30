using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class Level : IStructure
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }
        public int Number { get; set; }
        public PropertyCollection Properties { get; set; }

        public Level(string uniqueName, string caption, int number)
        {
            UniqueName = uniqueName;
            Caption = caption;
            Number = number;
            Properties = new PropertyCollection();
        }

        public Level(string uniqueName, string caption, int number, PropertyCollection properties)
        {
            UniqueName = uniqueName;
            Caption = caption;
            Number = number;
            Properties = properties;
        }

        public Level Clone()
        {
            return new Level(UniqueName, Caption, Number, Properties.Clone());
        }
    }
}
