using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class Table : IField
    {
        public string Name { get; private set; }

        public Table(string name)
        {
            Name = name;
        }

        public Table Clone()
        {
            var t = new Table(Name);
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
