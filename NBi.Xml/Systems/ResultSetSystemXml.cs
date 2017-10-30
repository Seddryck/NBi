using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Constraints;
using NBi.Core.ResultSet;
using NBi.Xml.Items.ResultSet;
using System.IO;

namespace NBi.Xml.Systems
{
    public class ResultSetSystemXml : AbstractSystemUnderTestXml
    {
        [XmlElement("row")]
        public List<RowXml> _rows { get; set; }

        public IList<IRow> Rows
        {
            get { return _rows.Cast<IRow>().ToList(); }
        }

        public IList<string> Columns
        {
            get
            {
                if (_rows.Count == 0)
                    return new List<string>();

                var names = new List<string>();
                var row = _rows[0];

                foreach (var cell in row.Cells)
                {
                    if (!string.IsNullOrEmpty(cell.ColumnName))
                        names.Add(cell.ColumnName);
                    else
                        names.Add(string.Empty);
                }
                return names;
            }
        }

        [XmlIgnore]
        public IContent Content
        {
            get
            {
                return new ResultSetBuilder.Content(Rows, Columns);
            }
        }

        [XmlAttribute("file")]
        public virtual string File { get; set; }

        public override BaseItem BaseItem
        {
            get
            {
                return Query;
            }
        }

        [XmlElement("query")]
        public virtual QueryXml Query { get; set; }

        public override ICollection<string> GetAutoCategories()
        {
            return new List<string>() { "Result-set" };
        }

        public ResultSetSystemXml()
        {
            _rows = new List<RowXml>();
        }
    }
}
