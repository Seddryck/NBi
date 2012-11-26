using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class MeasureRow
    {
        public string PerspectiveName { get; set; }
        public string MeasureGroupName { get; set; }
        public string UniqueName { get; set; }
        public string Caption { get; set; }
        public string DisplayFolder { get; set; }

        private MeasureRow()
        {

        }

        public static MeasureRow Load(AdomdDataReader dataReader)
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
                var row = new MeasureRow();
                row.PerspectiveName = perspectiveName;
                row.MeasureGroupName = (string)dataReader.GetValue(18);
                row.UniqueName = (string)dataReader.GetValue(4);
                row.Caption = (string)dataReader.GetValue(5);
                row.DisplayFolder = (string)dataReader.GetValue(19);
                
                return row;
            }
            else
                return null;
        }
    }
}
