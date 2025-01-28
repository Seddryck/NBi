using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace NBi.Xml.Decoration.Command;

public class WaitXml : DecorationCommandXml
{
    [XmlAttribute("milliseconds")]
    public string MilliSeconds { get; set; }
}
