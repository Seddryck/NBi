using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.ResultSet.Lookup;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Alteration.Lookup;

public class LookupReplaceXml : AlterationXml
{
    [XmlElement("missing")]
    public MissingXml Missing { get; set; }

    [XmlIgnore()]
    public bool MissingSpecified
    {
        get => Missing.Behavior != Behavior.Failure;
        set { }
    }

    [XmlElement("join")]
    public JoinXml Join { get; set; }

    [XmlElement("result-set")]
    public ResultSetSystemXml ResultSet { get; set; }

    [XmlElement("replacement")]
    public ColumnDefinitionLightXml Replacement { get; set; }

    public LookupReplaceXml()
        => Missing = new MissingXml() { Behavior = Behavior.Failure };
}
