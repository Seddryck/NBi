using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Settings;
using NBi.Model.Xml.Constraints;
using NBi.Core.ResultSet;
using NBi.Model.Xml.Items.ResultSet;
using System.IO;
using NBi.Model.Xml.Items.Alteration;

namespace NBi.Model.Xml.Systems;

public class ScalarXml : AbstractSystemUnderTestXml
{
    [XmlElement("query-scalar")]
    public virtual QueryScalarXml Query { get; set; }

    public override BaseItem BaseItem => Query;

    public override ICollection<string> GetAutoCategories() => new List<string>() { "Scalar" };
}
