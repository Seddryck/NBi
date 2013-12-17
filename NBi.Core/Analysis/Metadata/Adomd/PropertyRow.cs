using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class PropertyRow
    {
        public string PerspectiveName { get; set; }
        public string DimensionUniqueName { get; set; }
        public string HierarchyUniqueName { get; set; }
        public string LevelUniqueName { get; set; }
        public string UniqueName { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }

        private PropertyRow()
        {

        }

        public static PropertyRow Load(AdomdDataReader dataReader)
        {
            // Traverse the response and 
            // read column 2, "CUBE_NAME"
            // read column 3, "DIMENSION_UNIQUE_NAME"
            // read column 4, "HIERARCHY_UNIQUE_NAME"
            // read column 5, "LEVEL_UNIQUE_NAME"
            // read column 7, "PROPERTY_TYPE" (Must be 1)
            // read column 8, "PROPERTY_NAME"
            // read column 9, "PROPERTY_CAPTION"
            // read column 10, "DATA_TYPE" (int value)
            // read column 23, "PROPERTY_IS_VISIBLE"

            // Get the column value
            string perspectiveName = (string)dataReader.GetValue(2);
            if (!perspectiveName.StartsWith("$") && (bool)dataReader.GetValue(23) && ((short)dataReader.GetValue(7)) == 1)
            {
                // Get the columns value
                string dimensionUniqueName = (string)dataReader.GetValue(3);
                if (dimensionUniqueName != "[Measure]") //Needed to avoid dimension  previously filtered
                {
                    var row = new PropertyRow();
                    row.PerspectiveName = perspectiveName;
                    row.DimensionUniqueName = dimensionUniqueName;
                    row.HierarchyUniqueName = (string)dataReader.GetValue(4);
                    row.LevelUniqueName = (string)dataReader.GetValue(5);
                    row.Name = (string)dataReader.GetValue(8);
                    row.UniqueName = string.Format("{0}.[{1}]", row.LevelUniqueName, row.Name);
                    row.Caption = (string)dataReader.GetValue(9);

                    return row;
                }
                
            }
            return null;
        }
    }
}
