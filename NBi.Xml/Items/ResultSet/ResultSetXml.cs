using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;


namespace NBi.Xml.Items.ResultSet
{
    public class ResultSetXml : BaseItem
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
        public ResultSetBuilder.Content Content
        {
            get { return new ResultSetBuilder.Content(Rows, Columns); }
        }

        [XmlAttribute("file")]
        public string File { get; set; }

        public string GetFile()
        {
            var file = string.Empty;
            if (Path.IsPathRooted(File))
                file = File;
            else
                file = Settings.BasePath + File;

            return file;
        }

        public ResultSetXml()
        {
            _rows = new List<RowXml>();
        }

    }
}
