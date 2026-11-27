using System;
using System.Linq;
using System.IO;
using NBi.Core;

namespace NBi.Model.Decoration.Command;

public class FileDelete : IOAbstract
{
    public string FileName { get; set; }
    public string Path { get; set; }
}
