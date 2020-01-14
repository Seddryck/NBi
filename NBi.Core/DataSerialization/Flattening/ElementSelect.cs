using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Flattening
{
    public class ElementSelect : IPathSelect
    {
        public string Path { get; }

        internal ElementSelect(string path)
            => Path = path;
    }
}
