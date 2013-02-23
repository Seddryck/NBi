using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class HierarchyRow
    {
        public string PerspectiveName { get; set; }
        public string DimensionUniqueName { get; set; }
        public string UniqueName { get; set; }
        public string Caption { get; set; }

        private HierarchyRow()
        {

        }

        public static HierarchyRow Load(AdomdDataReader dataReader)
        {
            // Traverse the response and 
            // read column 2, "CUBE_NAME"
            // read column 3, "DIMENSION_UNIQUE_NAME"
            // read column 21, "HIERARCHY_IS_VISIBLE"
            // read column 5, "HIERARCHY_UNIQUE_NAME"
            // read column 7, "HIERARCHY_CAPTION"

            // Get the column value
            string perspectiveName = (string)dataReader.GetValue(2);
            if (!perspectiveName.StartsWith("$") && (bool)dataReader.GetValue(21))
            {
                if (true) //Needed to avoid dimension [Measure] previously filtered
                {
                    // Get the columns value
                    var row = new HierarchyRow();
                    row.PerspectiveName = perspectiveName;
                    row.DimensionUniqueName = (string)dataReader.GetValue(3);
                    row.UniqueName = (string)dataReader.GetValue(5);
                    row.Caption = (string)dataReader.GetValue(7);
                    return row;
                }
            }
            else
                return null;
        }
    }
}
