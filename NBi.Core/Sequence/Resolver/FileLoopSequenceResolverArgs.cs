using NBi.Core.IO.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver;

public class FileLoopSequenceResolverArgs(string basePath, string path) : ISequenceResolverArgs
{
    public string BasePath { get; set; } = basePath;
    public string Path { get; set; } = path;
    public IList<IFileFilter> Filters { get; set; } = [];
}
