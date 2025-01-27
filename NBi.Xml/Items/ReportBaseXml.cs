using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.Report;

namespace NBi.Xml.Items;

public class ReportBaseXml
{
    [XmlAttribute("source")]
    public string Source { get; set; }
    
    [XmlAttribute("path")]
    public string Path { get; set; }

    public ReportBaseXml()
    {

    }
}
