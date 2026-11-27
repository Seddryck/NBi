using System;
using System.Linq;
using System.IO;
using NBi.Core;

namespace NBi.Model.Decoration.Command;

public class FileDeletePattern : IOAbstract
{
    public string Path { get; set; }
    public string Pattern { get; set; }
}
