using NBi.Core.Sequence.Resolver.Loop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables.Sequence;

public class FileLoopXml
{
    [XmlAttribute("path")]
    public string Path { get; set; }

    [XmlAttribute("pattern")]
    public string Pattern { get; set; }
}
