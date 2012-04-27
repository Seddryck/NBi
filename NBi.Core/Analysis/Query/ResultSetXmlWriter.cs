using System.Data;
using System.IO;

namespace NBi.Core.Analysis.Query
{
    public class ResultSetXmlWriter : ResultSetAbstractWriter
    {

        public ResultSetXmlWriter(string persistancePath) :base(persistancePath) {}

    
        protected override void OnWrite(string filename, DataSet ds, string tableName)
        {
            var table = ds.Tables[tableName];

            table.WriteXml(Path.Combine(PersistencePath,filename), XmlWriteMode.WriteSchema, false);
        }
    }
}
