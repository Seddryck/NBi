using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command;

public class FileDeleteXml : IOAbstractXml
{
    [XmlAttribute("name")]
    public string FileName { get; set; }
    [XmlAttribute("path")]
    public string Path { get; set; }
}
