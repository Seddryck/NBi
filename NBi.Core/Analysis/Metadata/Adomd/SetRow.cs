using System;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class SetRow
    {
        public string PerspectiveName { get; set; }
        public string UniqueName { get; set; }
        public string Caption { get; set; }
        public string DisplayFolder { get; set; }
        private SetRow()
        {

        }

        public static SetRow Load(AdomdDataReader dataReader)
        {
            // read column 2, "CUBE_NAME"
            // read column 3, "SET_NAME"
            // read column 8, "SET_CAPTION"
            // read column 9, "SET_DISPLAY_FOLDER"

            var perspectiveName = (string)dataReader.GetValue(2);
            if (!perspectiveName.StartsWith("$"))
            {
                // Get the columns value
                var row = new SetRow();
                row.PerspectiveName = perspectiveName;
                row.UniqueName = (string)dataReader.GetValue(3);
                row.Caption = (string)dataReader.GetValue(8);
                row.DisplayFolder = (string)dataReader.GetValue(9);
                return row;
            }
            else
                return null;
        }
    }
}
