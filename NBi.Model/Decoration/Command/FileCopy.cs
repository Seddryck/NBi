using System;
using System.Linq;
using System.IO;
using NBi.Core;

namespace NBi.Model.Decoration.Command;

public class FileCopy : IOAbstract
{
    public string FileName { get; set; }
    public string DestinationPath { get; set; }
    public string SourcePath { get; set; }
}
