using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NBi.Core.Transformation.Transformer.Native.Text;

namespace NBi.Core.Transformation.Transformer.Native.IO
{
    abstract class AbstractPathTransformation : AbstractTextTransformation, IBasePathTransformation
    {
        protected string BasePath { get; }
        public AbstractPathTransformation(string basePath) => BasePath = basePath;
        protected override object EvaluateNull() => "(empty)";
        protected override object EvaluateEmpty() => "(empty)";
        protected override object EvaluateBlank() => "(empty)";
        protected override object EvaluateSpecial(string value) => "(empty)";
    }

    class PathToFilename : AbstractPathTransformation
    {
        public PathToFilename(string basePath) : base(basePath) { }
        protected override object EvaluateString(string value) => Path.GetFileName(value);
    }

    class PathToFilenameWithoutExtension : AbstractPathTransformation
    {
        public PathToFilenameWithoutExtension(string basePath) : base(basePath) { }
        protected override object EvaluateString(string value) => Path.GetFileNameWithoutExtension(value);
    }

    class PathToExtension : AbstractPathTransformation
    {
        public PathToExtension(string basePath) : base(basePath) { }
        protected override object EvaluateString(string value) => Path.GetExtension(value);
    }

    class PathToRoot : AbstractPathTransformation
    {
        public PathToRoot(string basePath) : base(basePath) { }
        protected override object EvaluateString(string value) 
            => Path.GetPathRoot(PathExtensions.CombineOrRoot(BasePath, value));
    }

    class PathToDirectory : AbstractPathTransformation
    {
        public PathToDirectory(string basePath) : base(basePath) { }
        protected override object EvaluateString(string value)
        {
            var fullPath = (Path.IsPathRooted(value) || string.IsNullOrEmpty(BasePath))
                ? value
                : Path.Combine(BasePath, value);
            return Path.GetDirectoryName(fullPath) == null ? Path.GetPathRoot(fullPath) : Path.GetDirectoryName(fullPath) + Path.DirectorySeparatorChar;
        }
            
    }
}
