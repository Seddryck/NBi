using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.IO;

public class FolderExistsConditionArgs : IIoConditionArgs
{
    public string BasePath { get; }
    public IScalarResolver<string> FolderName { get; }
    public IScalarResolver<string> FolderPath { get; }
    public IScalarResolver<bool> NotEmpty { get; }

    public FolderExistsConditionArgs(string basePath, IScalarResolver<string> folderPath, IScalarResolver<string> folderName, IScalarResolver<bool> notEmpty)
        => (BasePath, FolderPath, FolderName, NotEmpty) = (basePath, folderPath, folderName, notEmpty);
}
