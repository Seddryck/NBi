using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command;

public class FileCopyExtensionXml : IOAbstractXml
{
    [XmlAttribute("source-path")]
    public string SourcePath { get; set; }
    [XmlAttribute("destination-path")]
    public string DestinationPath { get; set; }
    [XmlAttribute("extension")]
    public string Extension { get; set; }
}
