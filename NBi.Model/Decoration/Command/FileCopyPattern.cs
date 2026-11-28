using System;
using System.Linq;
using System.IO;
using NBi.Core;

namespace NBi.Model.Decoration.Command;

public class FileCopyPattern : IOAbstract
{
    public string SourcePath { get; set; }
    public string DestinationPath { get; set; }
    public string Pattern { get; set; }
}
