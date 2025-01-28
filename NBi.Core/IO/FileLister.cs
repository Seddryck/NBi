using NBi.Core.IO.Filtering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.IO.File;

public class FileLister
{
    public string Path { get; }

    public FileLister(string path) => Path = path;

    public IEnumerable<FileInfo> Execute(IEnumerable<IFileFilter> filters)
    {
        var rootFilter = filters.OfType<IRootFileFilter>().SingleOrDefault() ?? new NullRootFilter();
        IEnumerable<FileInfo> files = rootFilter.Execute(Path);
        return files;
    }
}
