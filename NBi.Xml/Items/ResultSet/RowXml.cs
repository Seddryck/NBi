using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Core.ResultSet;

namespace NBi.Xml.Items.ResultSet;

public class RowXml : IRow
{
    [XmlElement("cell")]
    public List<CellXml> _cells { get; set; }
    
    public IList<ICell> Cells 
    {
        get { return _cells.Cast<ICell>().ToList(); }
    }

}
