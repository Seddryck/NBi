using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader;

public class FileReaderArgs : IReaderArgs
{
    public string BasePath { get; }
    public IScalarResolver<string> Path { get; }

    public FileReaderArgs(string basePath, IScalarResolver<string> path)
        => (BasePath, Path) = (basePath, path);
}
