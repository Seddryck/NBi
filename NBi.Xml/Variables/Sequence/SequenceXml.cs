using NBi.Core.ResultSet;
using NBi.Xml.Items;
using NBi.Xml.Variables.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables.Sequence;

public class SequenceXml
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("type")]
    public ColumnType Type { get; set; }

    [XmlElement("loop-sentinel")]
    public SentinelLoopXml? SentinelLoop { get; set; }

    [XmlElement("item")]
    public List<string> Items { get; set; } = new List<string>();

    [XmlElement("query-sequence")]
    public QueryXml? Query { get; set; }

    [XmlElement("loop-file")]
    public FileLoopXml? FileLoop { get; set; }

    [XmlElement("custom")]
    public CustomXml? Custom { get; set; }

    [XmlIgnore]
    public bool ItemsSpecified { get => Items.Count > 0; set { } }

    [XmlElement("filter")]
    public FilterSequenceXml? Filter { get; set; }

    [XmlIgnore]
    public bool FilterSpecified { get => Filter != null; set { } }
}
