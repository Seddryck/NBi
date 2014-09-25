using System.Collections.Generic;
using System.Data;
using System.Linq;

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
                                foreach (var l in h.Value.Levels)
                                {
                                    var p = l.Value.Properties.GetEnumerator();
                                    if (l.Value.Properties.Count > 0)
                                        p.MoveNext();
                                    do
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
                                        row[8] = l.Value.Caption;
                                        row[9] = l.Value.UniqueName;
                                        row[10] = l.Value.Number;
                                        row[11] = l.Value.Properties.Count > 0 ? p.Current.Value.Caption : string.Empty;
                                        row[12] = l.Value.Properties.Count > 0 ? p.Current.Value.UniqueName : string.Empty;
                                        dt.Rows.Add(row);
                                    } while (p.MoveNext());
                                    
                                }
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
            dt.Columns.Add(new DataColumn("LevelCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("LevelUniqueName", typeof(string)));
            dt.Columns.Add(new DataColumn("LevelNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("PropertyCaption", typeof(string)));
            dt.Columns.Add(new DataColumn("PropertyUniqueName", typeof(string)));
            return dt;
        }

        public static IEnumerable<string> GetReservedColumnNames()
        {
            return from column in MetadataFileFormat.CreateDataTable("").Columns.Cast<DataColumn>() select column.ColumnName;
        }
    }
}
