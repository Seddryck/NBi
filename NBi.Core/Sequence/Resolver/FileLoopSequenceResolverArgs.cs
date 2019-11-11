using NBi.Core.IO.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public class FileLoopSequenceResolverArgs : ISequenceResolverArgs
    {
        public string BasePath { get; set; }
        public string Path { get; set; }
        public IList<IFileFilter> Filters { get; set; } = new List<IFileFilter>();
    }
}
