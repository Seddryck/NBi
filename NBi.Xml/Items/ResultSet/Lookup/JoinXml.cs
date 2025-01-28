using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.ResultSet.Lookup;

public class JoinXml
{
    [XmlElement("mapping")]
    public List<ColumnMappingXml> Mappings { get; set; }

    [XmlElement("using")]
    public List<ColumnUsingXml> Usings { get; set; }

    public JoinXml()
    {
        Mappings = new List<ColumnMappingXml>();
        Usings = new List<ColumnUsingXml>();
    }
}
