using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command;

public class FileCopyXml : IOAbstractXml
{
    [XmlAttribute("name")]
    public string FileName { get; set; }
    [XmlAttribute("path")]
    public string DestinationPath { get; set; }
    [XmlAttribute("source-path")]
    public string SourcePath { get; set; }
}
