using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NBi.Core.Transformation.Transformer.Native.IO
{
    abstract class AbstractPathTransformation : AbstractTextTransformation
    {
        protected override object EvaluateNull() => "(empty)";
        protected override object EvaluateEmpty() => "(empty)";
        protected override object EvaluateBlank() => "(empty)";
        protected override object EvaluateSpecial(string value) => "(empty)";
    }

    class PathToFilename : AbstractPathTransformation
    {
        protected override object EvaluateString(string value) => Path.GetFileName(value);
    }

    class PathToFilenameWithoutExtension : AbstractPathTransformation
    {
        protected override object EvaluateString(string value) => Path.GetFileNameWithoutExtension(value);
    }

    class PathToExtension : AbstractPathTransformation
    {
        protected override object EvaluateString(string value) => Path.GetExtension(value);
    }

    class PathToRoot : AbstractPathTransformation
    {
        protected override object EvaluateString(string value) => Path.GetPathRoot(value);
    }

    class PathToDirectory : AbstractPathTransformation
    {
        protected override object EvaluateString(string value)
            => Path.GetDirectoryName(value)==null ? Path.GetPathRoot(value) : Path.GetDirectoryName(value) + Path.DirectorySeparatorChar;
    }
}
