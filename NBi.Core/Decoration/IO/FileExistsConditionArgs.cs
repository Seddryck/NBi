using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public class FileExistsConditionArgs : IIoConditionArgs
{
    public string BasePath { get; }
    public IScalarResolver<string> FolderName { get; }
    public IScalarResolver<string> FileName { get; }
    public IScalarResolver<bool> NotEmpty { get; }

    public FileExistsConditionArgs(string basePath, IScalarResolver<string> folderName, IScalarResolver<string> fileName, IScalarResolver<bool> notEmpty)
        => (BasePath, FolderName, FileName, NotEmpty) = (basePath, folderName, fileName, notEmpty);
}
