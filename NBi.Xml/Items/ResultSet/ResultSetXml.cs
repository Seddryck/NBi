using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Service;

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
        public IResultSetService Service
        {
            get
            {
                var factory = new ResultSetServiceFactory();

                if (string.IsNullOrEmpty(File))
                {
                    var content = new ResultSetBuilder.Content(Rows, Columns);
                    return factory.Instantiate(content, null);
                }
                else
                    return factory.Instantiate(GetFile(), null);
            }
        }

        [XmlAttribute("file")]
        public string File { get; set; }

        protected string GetFile()
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
