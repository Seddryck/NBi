using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class PerspectiveRow
    {
        public string Name { get; set; }

        private PerspectiveRow()
        {

        }

        public static PerspectiveRow Load(AdomdDataReader dataReader)
        {
            // Traverse the response and 
            // read column 2, "CUBE_NAME"
            // read column 4, "MEASURE_UNIQUE_NAME"
            // read column 5, "MEASURE_CAPTION"
            // read column 18, "MEASUREGROUP_NAME"

            // Get the column value
            string perspectiveName = (string)dataReader.GetValue(2);
            if (!perspectiveName.StartsWith("$"))
            {
                // Get the column value
                var row = new PerspectiveRow();
                row.Name = perspectiveName;
               
                return row;
            }
            else
                return null;
        }
    }
}
