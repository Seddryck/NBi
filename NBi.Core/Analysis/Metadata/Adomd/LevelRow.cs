using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class LevelRow
    {
        public string PerspectiveName { get; set; }
        public string DimensionUniqueName { get; set; }
        public string HierarchyUniqueName { get; set; }
        public string UniqueName { get; set; }
        public string Caption { get; set; }
        public int Number { get; set; }

        private LevelRow()
        {

        }

        public static LevelRow Load(AdomdDataReader dataReader)
        {
            // Traverse the response and 
            // read column 2, "CUBE_NAME"
            // read column 3, "DIMENSION_UNIQUE_NAME"
            // read column 4, "HIERARCHY_UNIQUE_NAME"
            // read column 6, "LEVEL_UNIQUE_NAME"
            // read column 8, "LEVEL_CAPTION"
            // read column 9, "LEVEL_NUMBER"
            // read column 15, "LEVEL_IS_VISIBLE"

            // Get the column value
            string perspectiveName = (string)dataReader.GetValue(2);
            if (!perspectiveName.StartsWith("$") && (bool)dataReader.GetValue(15))
            {
                string dimensionUniqueName = (string)dataReader.GetValue(3);
                if (true) //Needed to avoid dimension [Measure] previously filtered
                //Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)
                {
                    // Get the columns value
                    var row = new LevelRow();
                    row.PerspectiveName = perspectiveName;
                    row.DimensionUniqueName = dimensionUniqueName;
                    row.HierarchyUniqueName = (string)dataReader.GetValue(4);
                    row.UniqueName = (string)dataReader.GetValue(6);
                    row.Caption = (string)dataReader.GetValue(8);
                    row.Number = Convert.ToInt32((uint)dataReader.GetValue(9));
                    return row;
                }

            }
            else
                return null;
        }
    }
}
