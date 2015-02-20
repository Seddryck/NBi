using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class Column : IField
    {
        public string Name { get; private set; }

        public Column(string name)
        {
            Name = name;
        }

        public Column Clone()
        {
            var t = new Column(Name);
            return t;
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public string Caption
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }
    }
}
