using System;
using System.Linq;
using System.IO;
using NBi.Core;

namespace NBi.Model.Decoration.Command;

public class FileCopyExtension : IOAbstract
{
    public string SourcePath { get; set; }
    public string DestinationPath { get; set; }
    public string Extension { get; set; }
}
