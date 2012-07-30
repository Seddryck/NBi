using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Core.ResultSet;

namespace NBi.Xml.Constraints.EqualTo
{
    public class ResultSetXml
    {
        [XmlElement("row")]
        public List<RowXml> _rows { get; set; }

        public IList<IRow> Rows
        {
            get { return _rows.Cast<IRow>().ToList(); }
        }

        [XmlAttribute("file")]
        public string File { get; set; }

        public ResultSetXml()
        {
            _rows = new List<RowXml>();
        } 

    }
}
