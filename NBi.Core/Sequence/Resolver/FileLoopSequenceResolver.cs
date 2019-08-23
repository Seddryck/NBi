using NBi.Core.IO.File;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public class FileLoopSequenceResolver : ISequenceResolver<string>
    {
        private readonly FileLoopSequenceResolverArgs args;

        public FileLoopSequenceResolver(FileLoopSequenceResolverArgs args) => this.args = args;

        object IResolver.Execute() => Execute();
        IList ISequenceResolver.Execute() => Execute();

        public List<string> Execute()
        {
            var fileLister = new FileLister(PathExtensions.CombineOrRoot(args.BasePath, args.Path));
            var files = fileLister.Execute(args.Filters);
            return files.Select(x => x.Name).ToList();
        }

        
    }
}
