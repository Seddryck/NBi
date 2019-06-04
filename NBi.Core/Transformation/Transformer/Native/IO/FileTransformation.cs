using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NBi.Core.Transformation.Transformer.Native.IO
{
    abstract class AbstractFileTransformation : AbstractTextTransformation, IBasePathTransformation
    {
        protected string BasePath { get; }
        public AbstractFileTransformation(string basePath) => BasePath = basePath;
        protected override object EvaluateNull() => throw new NBiException("Can't evaluate a property a file when the path is equal to (null)");
        protected override object EvaluateEmpty() => throw new NBiException("Can't evaluate a property a file when the path is equal to (empty)");
        protected override object EvaluateBlank() => throw new NBiException("Can't evaluate a property a file when the path is equal to (blank)");
        protected override object EvaluateSpecial(string value) => throw new NBiException("Can't evaluate a property a file when the path is equal to a special value");
        protected override object EvaluateString(string value)
        {
            var fullPath = Path.Combine(BasePath, value);
            var fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Exists)
                throw new ExternalDependencyNotFoundException(fullPath);
            return EvaluateFileInfo(fileInfo);
        }

        protected abstract object EvaluateFileInfo(FileInfo value);
    }

    class FileToSize : AbstractFileTransformation
    {
        public FileToSize(string basePath) : base(basePath) { }
        protected override object EvaluateFileInfo(FileInfo value) => value.Length;
    }

    class FileToCreationDateTime : AbstractFileTransformation
    {
        public FileToCreationDateTime(string basePath) : base(basePath) { }
        protected override object EvaluateFileInfo(FileInfo value) => value.CreationTime;
    }

    class FileToCreationDateTimeUtc : AbstractFileTransformation
    {
        public FileToCreationDateTimeUtc(string basePath) : base(basePath) { }
        protected override object EvaluateFileInfo(FileInfo value) => value.CreationTimeUtc;
    }

    class FileToUpdateDateTime : AbstractFileTransformation
    {
        public FileToUpdateDateTime(string basePath) : base(basePath) { }
        protected override object EvaluateFileInfo(FileInfo value) => value.LastWriteTime;
    }

    class FileToUpdateDateTimeUtc : AbstractFileTransformation
    {
        public FileToUpdateDateTimeUtc(string basePath) : base(basePath) { }
        protected override object EvaluateFileInfo(FileInfo value) => value.LastWriteTimeUtc;
    }
}
