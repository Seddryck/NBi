using System.IO;
using System.Text;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public class ResultSetCsvReader
    {
        public CsvDefinition Definition { get; private set; }

        public ResultSetCsvReader() : this (CsvDefinition.SemiColumnDoubleQuote())
        {
        }

        public ResultSetCsvReader(CsvDefinition definition)
        {
            Definition = definition;
        }
  
        public ResultSet Read(string fullpath)
        {
            string res = null;

            var file = Path.Combine(fullpath);
            using (StreamReader infile = new StreamReader(file, Encoding.UTF7))
            {
                res = infile.ReadToEnd();
            }

            return Parse(res);
        }

        public ResultSet Parse(string raw)
        {
            raw = raw.Replace(Definition.TextQualifier.ToString(), "");
            var rows = raw.Split(new string[] {"\r\n"}, System.StringSplitOptions.RemoveEmptyEntries);
            var rowfields = rows.Select<string, string[]>( 
                r=> r.Split(new char[] { Definition.FieldSeparator }, System.StringSplitOptions.RemoveEmptyEntries)
                );

            var rs = new ResultSet();
            rs.Load(rowfields);
            return rs;
        }
    }
}
