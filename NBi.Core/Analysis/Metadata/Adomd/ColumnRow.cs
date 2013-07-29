using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class ColumnRow
    {
        public string Name { get; set; }

        private ColumnRow()
        {

        }

        public static ColumnRow Load(AdomdDataReader dataReader)
        {
            // Traverse the response and 
            // read column 1, "TABLE_SCHEMA"
            // read column 2, "TABLE_NAME"
            // read column 2, "COLUMN_NAME"

            // Get the column value
            string perspectiveName = dataReader.GetString(1);
            string tableName = dataReader.GetString(2);
            string columnName = dataReader.GetString(3);
            if (tableName.StartsWith("$") && columnName!="RowNumber")
            {
                // Get the column value
                var row = new ColumnRow();
                row.Name = columnName;
               
                return row;
            }
            else
                return null;
        }
    }
}
