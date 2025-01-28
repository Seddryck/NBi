using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command;

public class FileDeletePatternXml : IOAbstractXml
{
    [XmlAttribute("path")]
    public string Path { get; set; }
    [XmlAttribute("pattern")]
    public string Pattern { get; set; }
}
