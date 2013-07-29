using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class DimensionRow
    {
        public string PerspectiveName { get; set; }
        public string UniqueName { get; set; }
        public string Caption { get; set; }
        public string DefaultHierarchy { get; set; }

        private DimensionRow()
        {

        }

        public static DimensionRow Load(AdomdDataReader dataReader)
        {
            // read column 2, "CUBE_NAME"
            // read column 4, "DIMENSION_UNIQUE_NAME"
            // read column 6, "DIMENSION_CAPTION"
            // read column 8, "DIMENSION_TYPE"
            // read column 10, "DEFAULT HIERARCHY"

            var perspectiveName = (string)dataReader.GetValue(2);
            if ((short)dataReader.GetValue(8) != 2 && !perspectiveName.StartsWith("$"))
            {
                // Get the columns value
                var row = new DimensionRow();
                row.PerspectiveName = perspectiveName;
                row.UniqueName = (string)dataReader.GetValue(4);
                row.Caption = (string)dataReader.GetValue(6);
                row.DefaultHierarchy = (string)dataReader.GetValue(10);
                return row;
            }
            else
                return null;
        }

        public static DimensionRow LoadLinkedTo(AdomdDataReader dataReader)
        {
            // read column 2, "CUBE_NAME"
            // read column 5, "DIMENSION_UNIQUE_NAME"
            // read column 6, "DIMENSION_CAPTION"
            // read column 7, "DIMENSION_IS_VISIBLE"

            var perspectiveName = (string)dataReader.GetValue(2);
            if ((bool)dataReader.GetValue(7) && !perspectiveName.StartsWith("$"))
            {
                // Get the columns value
                var row = new DimensionRow();
                row.PerspectiveName = perspectiveName;
                row.UniqueName = dataReader.GetString(5);
                row.Caption = dataReader.GetString(5).Replace("[", "").Replace("]", "");
                return row;
            }
            else
                return null;
        }
    }
}
