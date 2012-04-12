using System.Data;
using System.IO;
using System.Text;

namespace NBi.Core.Analysis.Query
{
    public class ResultSetCsvReader
    {
        public CsvDefinition Definition { get; private set; }

        public string PersistencePath { get; private set; }

        public ResultSetCsvReader(string persistancePath)
        {
            PersistencePath = persistancePath;
            Definition = CsvDefinition.SemiColumnDoubleQuote();
        }

        public ResultSetCsvReader(string persistancePath, CsvDefinition definition)
            : this(persistancePath)
        {
            Definition = definition;
        }
  
        public string Read(string filename)
        {
            string res = null;

            var file = Path.Combine(Path.GetFullPath(PersistencePath), filename);
            using (StreamReader infile = new StreamReader(file, Encoding.UTF8))
            {
                res = infile.ReadToEnd();
            }

            return res;
        }
    }
}
