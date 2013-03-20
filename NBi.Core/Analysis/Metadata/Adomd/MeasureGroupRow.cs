using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class MeasureGroupRow
    {
        public string PerspectiveName { get; set; }
        public string Name { get; set; }
        public string LinkedDimensionUniqueName { get; set; }

        private MeasureGroupRow()
        {

        }

        public static MeasureGroupRow Load(AdomdDataReader dataReader)
        {
            // Traverse the response and 
            // Traverse the response and 
            // read column 2, "CUBE_NAME"
            // read column 3, "MEASUREGROUP_NAME"
            // read column 5, "DIMENSION_UNIQUE_NAME"
            // read column 7, "DIMENSION_IS_VISIBLE"

            // Get the column value
            string perspectiveName = (string)dataReader.GetValue(2);
            if (!perspectiveName.StartsWith("$"))
            {
                var row = new MeasureGroupRow();
                // Get the column value
                row.PerspectiveName = perspectiveName;
                row.Name = (string)dataReader.GetValue(3);
                
                //Check if the linked dimension is visible
                if ((bool)dataReader.GetValue(7))
                    row.LinkedDimensionUniqueName = (string)dataReader.GetValue(5);
                return row;
            }
            else
                return null;
        }
    }
}
