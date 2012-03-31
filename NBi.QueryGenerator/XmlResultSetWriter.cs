using System.Data;
using System.IO;

namespace NBi.QueryGenerator
{
    public class XmlResultSetWriter : AbstractResultSetWriter
    {
        public string PersistencePath { get; private set; }

        public XmlResultSetWriter(string persistancePath) :base(persistancePath) {}

    
        protected override void OnWrite(string filename, DataSet ds, string tableName)
        {
            var table = ds.Tables[tableName];

            table.WriteXml(Path.Combine(PersistencePath,filename), XmlWriteMode.WriteSchema, false);
        }
    }
}
