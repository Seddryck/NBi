using System;
using System.Linq;
using System.IO;
using NBi.Core;
using NBi.Extensibility;

namespace NBi.Model.Decoration.Command;

public class TableLoad : DataManipulationAbstract
{
    public string TableName { get; set; }

    public string InternalFileName { get; set; }

    public string FileName
    {
        get
        {
            var file = string.Empty;
            if (Path.IsPathRooted(InternalFileName))
                file = InternalFileName;
            else
                file = (Settings?.BasePath ?? string.Empty) + InternalFileName;

            return file;
        }
    }
}
