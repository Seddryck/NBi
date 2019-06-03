using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NBi.Core.Transformation.Transformer.Native.IO
{
    abstract class AbstractFileTransformation : AbstractTextTransformation
    {
        protected override object EvaluateNull() => throw new NBiException("Can't evaluate a property a file when the path is equal to (null)");
        protected override object EvaluateEmpty() => throw new NBiException("Can't evaluate a property a file when the path is equal to (empty)");
        protected override object EvaluateBlank() => throw new NBiException("Can't evaluate a property a file when the path is equal to (blank)");
        protected override object EvaluateSpecial(string value) => throw new NBiException("Can't evaluate a property a file when the path is equal to a special value");
        protected override object EvaluateString(string value)
        {
            var fileInfo = new FileInfo(value);
            if (!fileInfo.Exists)
                throw new ExternalDependencyNotFoundException(value);
            return EvaluateFileInfo(fileInfo);
        }

        protected abstract object EvaluateFileInfo(FileInfo value);
    }

    class FileToSize : AbstractFileTransformation
    {
        protected override object EvaluateFileInfo(FileInfo value) => value.Length;
    }

    class FileToCreationDateTime : AbstractFileTransformation
    {
        protected override object EvaluateFileInfo(FileInfo value) => value.CreationTime;
    }

    class FileToCreationDateTimeUtc : AbstractFileTransformation
    {
        protected override object EvaluateFileInfo(FileInfo value) => value.CreationTimeUtc;
    }

    class FileToUpdateDateTime : AbstractFileTransformation
    {
        protected override object EvaluateFileInfo(FileInfo value) => value.LastWriteTime;
    }

    class FileToUpdateDateTimeUtc : AbstractFileTransformation
    {
        protected override object EvaluateFileInfo(FileInfo value) => value.LastWriteTimeUtc;
    }
}
