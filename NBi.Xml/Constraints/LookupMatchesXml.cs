using NBi.Xml.Items.ResultSet;
using NBi.Xml.Items.ResultSet.Lookup;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Constraints;

public class LookupMatchesXml : AbstractConstraintXml
{
    [XmlElement("join")]
    public JoinXml? Join { get; set; }

    [XmlElement("inclusion")]
    public InclusionXml? Inclusion { get; set; }

    [XmlElement("result-set")]
    public ResultSetSystemXml? ResultSet { get; set; }

    [Obsolete("Replaced by result-set")]
    [XmlIgnore()]
    public ResultSetSystemXml? ResultSetOld
    {
        get => ResultSet;
        set { ResultSet = value; }
    }

    [XmlIgnore()]
    public override DefaultXml? Default
    {
        get { return base.Default; }
        set
        {
            base.Default = value;
            if (ResultSet != null)
                ResultSet.Default = value;
        }
    }

}
