using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class TableRow
    {
        public string Name { get; set; }

        private TableRow()
        {

        }

        public static TableRow Load(AdomdDataReader dataReader)
        {
            // Traverse the response and 
            // read column 1, "TABLE_SCHEMA"
            // read column 2, "TABLE_NAME"

            // Get the column value
            string perspectiveName = dataReader.GetString(1);
            string tableName = dataReader.GetString(2);
            if (tableName.StartsWith("$"))
            {
                // Get the column value
                var row = new TableRow();
                row.Name = tableName.Substring(1);
               
                return row;
            }
            else
                return null;
        }
    }
}
