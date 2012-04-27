using System.Data;
using System.IO;
using System.Text;

namespace NBi.Core.Analysis.Query
{
    public class ResultSetCsvWriter : ResultSetAbstractWriter
    {
        public CsvDefinition Definition { get; private set; }

        public ResultSetCsvWriter(string persistancePath) :base(persistancePath)
        {
            Definition = CsvDefinition.SemiColumnDoubleQuote();
        }

        public ResultSetCsvWriter(string persistancePath, CsvDefinition definition) :base(persistancePath)
        {
            Definition = definition;
        }
  
        protected override void OnWrite(string filename, DataSet ds, string tableName)
        {
            var table = ds.Tables[tableName];
            var str = BuildContent(table);

            var file = Path.Combine(Path.GetFullPath(PersistencePath), filename);
            using (StreamWriter outfile = new StreamWriter(file, false, Encoding.UTF8))
            {
                outfile.Write(str);
            }
        }

        public string BuildContent(DataTable table)
        {
            var sb = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    var item = row.ItemArray[i];

                    if (item.GetType() == typeof(string))
                        ((string)item).Replace(Definition.TextQualifier.ToString(), Definition.TextQualifier.ToString() + Definition.TextQualifier.ToString());

                    sb.AppendFormat("\"{0}\"{1}", item, Definition.FieldSeparator);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        
    }
}
