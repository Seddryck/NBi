using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataExcelOleDbReader : MetadataExcelOleDbAbstract, IMetadataReader
    {

        public string SheetRange { get; set; }

        protected DataTable _dataTable;
        protected DataTable DataTable
        {
            get
            {
                if (_dataTable == null)
                    _dataTable = LoadDataTable();
                return _dataTable;
            }
        }

        protected int _reservedColumnsCount;
        protected List<string> _tracks;
        public IEnumerable<string> Tracks
        {
            get { return _tracks; }
        }


        public MetadataExcelOleDbReader(string filename) : base(filename) { }


        public MetadataExcelOleDbReader(string filename, string sheetname) : base(filename, sheetname) { }
        
        
        public MetadataExcelOleDbReader(string filename, string sheetname, string sheetRange) : base(filename,sheetname)
        {
            SheetRange = sheetRange;
        }

        public CubeMetadata Read(string track)
        {
            CubeMetadata metadata=new CubeMetadata();

            RaiseProgressStatus((string.Format("Processing Xls file for track {0}", track)));

            int i = 0;
            foreach (DataRow row in DataTable.Rows)
	        {
                i++;
                RaiseProgressStatus("Loading row {0} of {1}", i, DataTable.Rows.Count);
                var trackPos = _tracks.IndexOf(track) + _reservedColumnsCount;
                var r = GetMetadata(row, trackPos);

                LoadMetadata(r, true, ref metadata);
            }

            RaiseProgressStatus("Xls file processed");

            return metadata;
        }

        public CubeMetadata Read()
        {
            CubeMetadata metadata = new CubeMetadata();

            RaiseProgressStatus("Processing Xls file");
            int i = 0;
            foreach (DataRow row in DataTable.Rows)
            {
                i++;
                RaiseProgressStatus("Loading row {0} of {1}", i, DataTable.Rows.Count);
                var r = GetMetadata(row);

                LoadMetadata(r, false, ref metadata);
            }
            RaiseProgressStatus("Xls file processed");

            return metadata;
        }

        private void LoadMetadata(XlsMetadata r, bool filter, ref CubeMetadata metadata)
        {
            MeasureGroup mg = null;

            if ((!filter) || r.isChecked)
            {
                metadata.Perspectives.AddOrIgnore(r.perspectiveName);
                var perspective = metadata.Perspectives[r.perspectiveName];

                if (perspective.MeasureGroups.ContainsKey(r.measureGroupName))
                {
                    mg = perspective.MeasureGroups[r.measureGroupName];
                }
                else
                {
                    mg = new MeasureGroup(r.measureGroupName);
                    perspective.MeasureGroups.Add(mg);
                }

                if (!mg.Measures.ContainsKey(r.measureUniqueName))
                {
                    mg.Measures.Add(r.measureUniqueName, r.measureCaption, r.measureDisplayFolder);
                }

                Dimension dim = null;

                if (perspective.Dimensions.ContainsKey(r.dimensionUniqueName))
                {
                    dim = perspective.Dimensions[r.dimensionUniqueName];
                }
                else
                {
                    dim = new Dimension(r.dimensionUniqueName, r.dimensionCaption, new HierarchyCollection());
                    perspective.Dimensions.Add(dim);
                }

                if (!dim.Hierarchies.ContainsKey(r.hierarchyUniqueName))
                {
                    var hierarchy = new Hierarchy(r.hierarchyUniqueName, r.hierarchyCaption, string.Empty);
                    dim.Hierarchies.Add(r.hierarchyUniqueName, hierarchy);
                }

                if (r.levelUniqueName != null)
                {
                    if (!dim.Hierarchies[r.hierarchyUniqueName].Levels.ContainsKey(r.levelUniqueName))
                    {
                        var level = new Level(r.levelUniqueName, r.levelCaption, r.levelNumber);
                        dim.Hierarchies[r.hierarchyUniqueName].Levels.Add(r.levelUniqueName, level);
                    }

                    if (!string.IsNullOrEmpty(r.propertyUniqueName))
                    {
                        if (!dim.Hierarchies[r.hierarchyUniqueName].Levels[r.levelUniqueName].Properties.ContainsKey(r.propertyUniqueName))
                        {
                            var prop = new Property(r.propertyUniqueName, r.propertyCaption);
                            dim.Hierarchies[r.hierarchyUniqueName].Levels[r.levelUniqueName].Properties.Add(r.propertyUniqueName, prop);
                        }
                    }
                }
                if (!mg.LinkedDimensions.ContainsKey(r.dimensionUniqueName))
                    mg.LinkedDimensions.Add(dim);
            }
        }

        protected DataTable LoadDataTable()
        {
            RaiseProgressStatus("Reading Xls file");
            var dt = new DataTable("Metadata");
        
            using (var conn = new OleDbConnection())
            {
                conn.ConnectionString = GetConnectionString(Filename);
                conn.Open();

                string commandText = null;
                if (string.IsNullOrEmpty(SheetRange))
                    commandText = string.Format("SELECT * FROM [{0}$]", SheetName);
                else
                    commandText = string.Format("SELECT * FROM [{0}]${1}", SheetName, SheetRange);

                using (var cmd = new OleDbCommand(commandText, conn))
                {

                    var adapter = new OleDbDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.FillSchema(dt, SchemaType.Source);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public void GetTracks()
        {
            var dt = new DataTable("Track");

            using (var conn = new OleDbConnection())
            {
                conn.ConnectionString = GetConnectionString(Filename);
                conn.Open();

                var commandText = string.Format("SELECT * FROM [{0}$A1:AZ1]", SheetName, SheetRange);

                using (var cmd = new OleDbCommand(commandText, conn))
                {

                    var adapter = new OleDbDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.FillSchema(dt, SchemaType.Source);
                    adapter.Fill(dt);
                }
            }

            _reservedColumnsCount = 0;
            _tracks = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                if (!MetadataFileFormat.GetReservedColumnNames().Contains(col.ColumnName))
                    _tracks.Add(col.ColumnName);
                else
                    _reservedColumnsCount++;
            }
        }

        protected XlsMetadata GetMetadata(DataRow row, int trackPos)
        {
            var res = GetMetadataBasic(row);

            if (trackPos>=0)
                res.isChecked = !row.IsNull(trackPos);
            return res;
        }

        protected XlsMetadata GetMetadata(DataRow row)
        {
            return GetMetadata(row, -1);
        }

        protected XlsMetadata GetMetadataBasic(DataRow row)
        {
            var xlsMetadata = new XlsMetadata();

            xlsMetadata.perspectiveName = (string)row[0];
            
            xlsMetadata.measureGroupName = row.IsNull("MeasureGroup") ? string.Empty : (string)row["MeasureGroup"];
            
            if (row.Table.Columns.IndexOf("Measure") > 0)
                xlsMetadata.measureCaption = (string)row["Measure"];
            if (row.Table.Columns.IndexOf("MeasureCaption") > 0)
                xlsMetadata.measureCaption = (string)row["MeasureCaption"];
            
            if(row.Table.Columns.IndexOf("MeasureUniqueName")>0)
                xlsMetadata.measureUniqueName = (string)row["MeasureUniqueName"];
            else
                xlsMetadata.measureUniqueName = "[" + (string)row["Measure"] + "]";

            if (row.Table.Columns.IndexOf("MeasureDisplayFolder") > 0)
                xlsMetadata.measureUniqueName = (string)row["MeasureDisplayFolder"];

            if (row.Table.Columns.IndexOf("DimensionCaption") > 0)
                xlsMetadata.dimensionCaption = (string)row["DimensionCaption"];
            else
                xlsMetadata.dimensionCaption = ((string)row["Dimension"]).Replace("[", "").Replace("]", "");

            if (row.Table.Columns.IndexOf("Dimension") > 0)
                xlsMetadata.dimensionUniqueName = (string)row["Dimension"];
            if (row.Table.Columns.IndexOf("DimensionUniqueName") > 0)
                xlsMetadata.dimensionUniqueName = (string)row["DimensionUniqueName"];

            if (row.Table.Columns.IndexOf("DimensionAttribute") > 0)
            {
                xlsMetadata.hierarchyCaption = (string)row["DimensionAttribute"];
                xlsMetadata.hierarchyUniqueName = "[" + (string)row["DimensionAttribute"] + "]";
            }
            if (row.Table.Columns.IndexOf("HierarchyCaption") > 0)
                xlsMetadata.hierarchyCaption = (string)row["HierarchyCaption"];
            if (row.Table.Columns.IndexOf("HierarchyUniqueName") > 0)
                xlsMetadata.hierarchyUniqueName = (string)row["HierarchyUniqueName"];

            if (row.Table.Columns.IndexOf("LevelCaption") > 0)
                xlsMetadata.levelCaption = (string)row["LevelCaption"];
            if (row.Table.Columns.IndexOf("LevelUniqueName") > 0)
                xlsMetadata.levelUniqueName = (string)row["LevelUniqueName"];
            if (row.Table.Columns.IndexOf("LevelNumber") > 0)
                xlsMetadata.levelNumber = int.Parse(row["LevelNumber"].ToString());

            if (row.Table.Columns.IndexOf("PropertyCaption") > 0)
                if (!row.IsNull("PropertyCaption"))
                    xlsMetadata.propertyCaption = (string)row["PropertyCaption"];
            if (row.Table.Columns.IndexOf("PropertyUniqueName") > 0)
                if (!row.IsNull("PropertyUniqueName"))
                    xlsMetadata.propertyUniqueName = (string)row["PropertyUniqueName"];

            return xlsMetadata;
        }

        protected struct XlsMetadata
        {
            public string perspectiveName;
            public string measureGroupName;
            public string measureCaption;
            public string measureUniqueName;
            public string measureDisplayFolder;
            public string dimensionCaption;
            public string dimensionUniqueName;
            public string hierarchyCaption;
            public string hierarchyUniqueName;
            public string levelCaption;
            public string levelUniqueName;
            public int levelNumber;
            public string propertyCaption;
            public string propertyUniqueName;
            public bool isChecked;
        }

       


    }
}
