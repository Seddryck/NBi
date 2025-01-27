using NBi.Core.IO.File;
using NBi.Extensibility.Resolving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver;

public class FileLoopSequenceResolver : ISequenceResolver<string>
{
    protected FileLoopSequenceResolverArgs Args { get; }

    public FileLoopSequenceResolver(FileLoopSequenceResolverArgs args) => Args = args;

    object IResolver.Execute() => Execute();
    IList ISequenceResolver.Execute() => Execute();

    public List<string> Execute()
    {
        var fileLister = new FileLister(PathExtensions.CombineOrRoot(Args.BasePath, Args.Path));
        var files = fileLister.Execute(Args.Filters);
        return files.Select(x => x.Name).ToList();
    }
}
