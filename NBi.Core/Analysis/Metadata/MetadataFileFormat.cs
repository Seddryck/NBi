using System.Data;

namespace NBi.Core.Analysis.Metadata
{
    internal class MetadataFileFormat
    {
        internal static DataTable WriteInDataTable(CubeMetadata metadata, string sheetName)
        {
            var dt = CreateDataTable(sheetName);

            foreach (var perspective in metadata.Perspectives)
            {
                foreach (var mg in perspective.Value.MeasureGroups)
                {
                    foreach (var m in mg.Value.Measures)
                    {
                        foreach (var dim in mg.Value.LinkedDimensions)
                        {
                            foreach (var h in dim.Value.Hierarchies)
                            {
                                var row = dt.NewRow();
                                row[0] = perspective.Value.Name;
                                row[1] = mg.Value.Name;
                                row[2] = m.Value.Caption;
                                row[3] = m.Value.UniqueName;
                                row[4] = dim.Value.Caption;
                                row[5] = dim.Value.UniqueName;
                                row[6] = h.Value.Caption;
                                row[7] = h.Value.UniqueName;
                                dt.Rows.Add(row);
                            }
                        }
                    }

                }
            }
            return dt;
        }

        internal static DataTable WriteInDataTable(CubeMetadata metadata)
        {
            return WriteInDataTable(metadata, "MyMetadata");
        }

        internal static DataTable CreateDataTable(string sheetName)
        {
            var dt = new DataTable(sheetName);
            dt.Columns.Add(new DataColumn("Perspective", typeof(string)));
            dt.Columns.Add(new DataColumn("MeasureGroup", typeof(string)));
            dt.Columns.Add(new DataColumn("MeasureCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("MeasureUniqueName", typeof(string)));
            dt.Columns.Add(new DataColumn("DimensionCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("DimensionUniqueName", typeof(string)));
            dt.Columns.Add(new DataColumn("HierarchyCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("HierarchyUniqueName", typeof(string)));
            return dt;
        }         
    }
}
