using NBi.Extensibility.Resolving;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader;

class FileReader : IDataSerializationReader, IDisposable
{
    private StreamReader? StreamReader { get; set; }
    public string BasePath { get; }
    public IScalarResolver<string> ResolverPath { get; }

    public FileReader( string basePath, IScalarResolver<string> resolverPath)
        => (BasePath, ResolverPath) = (basePath, resolverPath);

    public TextReader Execute()
    {
        var filePath = EnsureFileExist();
        StreamReader = new StreamReader(filePath);
        return StreamReader;
    }

    protected virtual string EnsureFileExist()
    {
        var filePath = PathExtensions.CombineOrRoot(BasePath, string.Empty, ResolverPath.Execute() ?? string.Empty);
        if (!File.Exists(filePath))
            throw new ExternalDependencyNotFoundException(filePath);
        return filePath;
    }

    bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            StreamReader?.Dispose();
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
