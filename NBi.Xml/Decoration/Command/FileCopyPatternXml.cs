using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using NBi.Core;

namespace NBi.Xml.Decoration.Command;

public class FileCopyPatternXml : IOAbstractXml
{
    [XmlAttribute("source-path")]
    public string SourcePath { get; set; }
    [XmlAttribute("destination-path")]
    public string DestinationPath { get; set; }
    [XmlAttribute("pattern")]
    public string Pattern { get; set; }
}
