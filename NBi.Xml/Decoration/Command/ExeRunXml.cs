using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace NBi.Xml.Decoration.Command;

public class ExeRunXml : DecorationCommandXml
{
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("path")]
    public string Path { get; set; }

    [XmlAttribute("arguments")]
    public string Arguments { get; set; }

    [XmlIgnore]
    public string Argument { get { return Arguments; } }

    [XmlAttribute("timeout-milliseconds")]
    [DefaultValue("0")]
    public string TimeOut { get; set; }

    public ExeRunXml()
    {
        TimeOut = "0";
    }
}
