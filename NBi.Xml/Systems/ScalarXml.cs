using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Constraints;
using NBi.Core.ResultSet;
using NBi.Xml.Items.ResultSet;
using System.IO;
using NBi.Xml.Items.Alteration;

namespace NBi.Xml.Systems;

public class ScalarXml : AbstractSystemUnderTestXml
{
    [XmlElement("query-scalar")]
    public virtual QueryScalarXml Query { get; set; }

    public override BaseItem BaseItem => Query;

    public override ICollection<string> GetAutoCategories() => new List<string>() { "Scalar" };
}
