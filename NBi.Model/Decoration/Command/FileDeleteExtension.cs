using System;
using System.Linq;
using System.IO;
using NBi.Core;

namespace NBi.Model.Decoration.Command;

public class FileDeleteExtension : IOAbstract
{
    public string Path { get; set; }
    public string Extension { get; set; }
}
