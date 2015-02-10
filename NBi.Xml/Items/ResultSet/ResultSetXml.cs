using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Core.ResultSet;
using System.ComponentModel;

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

        [XmlAttribute("file")]
        public string File { get; set; }

        [XmlAttribute("type")]
        [DefaultValue(ResultSetFileType.Csv)]
        public ResultSetFileType Type { get; set; }

        public ResultSetFile GetFile()
        {
            var path = string.Empty;
            if (Path.IsPathRooted(File))
                path = File;
            else
                path = Settings.BasePath + File;
            if (!System.IO.File.Exists(path))
                throw new ExternalDependencyNotFoundException(path);

            var file = new ResultSetFile(path, Type);

            return file;
        }

        public ResultSetXml()
        {
            _rows = new List<RowXml>();
            Type = ResultSetFileType.Csv;
        } 

    }
}
