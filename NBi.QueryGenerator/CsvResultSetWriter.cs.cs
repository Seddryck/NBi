using System.Data;
using System.IO;
using System.Text;

namespace NBi.QueryGenerator
{
    public class CsvResultSetWriter : AbstractResultSetWriter
    {
        public CsvDefinition Definition { get; private set; }

        public CsvResultSetWriter(string persistancePath) :base(persistancePath)
        {
            Definition = CsvDefinition.SemiColumnDoubleQuote();
        }

        public CsvResultSetWriter(string persistancePath, CsvDefinition definition) :base(persistancePath)
        {
            Definition = definition;
        }
  
        protected override void OnWrite(string filename, DataSet ds, string tableName)
        {
            var table = ds.Tables[tableName];
            var sb = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    var item = (string) row.ItemArray[i];
                    item.Replace( Definition.TextQualifier.ToString(), Definition.TextQualifier.ToString() + Definition.TextQualifier.ToString()); 
                    sb.AppendFormat("\"{0}\"{1}", item, Definition.FieldSeparator);
                }
                sb.AppendLine();
            }
            var file = Path.Combine(Path.GetFullPath(PersistencePath), filename);
            using (StreamWriter outfile = new StreamWriter(file, false, Encoding.UTF8))
            {
                outfile.Write(sb);
            }
        }
    }
}
