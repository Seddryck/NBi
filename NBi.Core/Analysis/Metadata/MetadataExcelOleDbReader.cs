using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataExcelOleDbReader : MetadataExcelOleDbAbstract
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

        public IList<string> Tracks { get; private set; }


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
                var trackPos = Tracks.IndexOf(track) + 6;
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
                    mg.Measures.Add(r.measureUniqueName, r.measureCaption);
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
                    var hierarchy = new Hierarchy(r.hierarchyUniqueName, r.hierarchyCaption);
                    dim.Hierarchies.Add(r.hierarchyUniqueName, hierarchy);
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

            Tracks = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                if (col.Ordinal>5)
                    Tracks.Add(col.ColumnName);
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

            return xlsMetadata;
        }

        protected struct XlsMetadata
        {
            public string perspectiveName;
            public string measureGroupName;
            public string measureCaption;
            public string measureUniqueName;
            public string dimensionCaption;
            public string dimensionUniqueName;
            public string hierarchyCaption;
            public string hierarchyUniqueName;
            public bool isChecked;
        }

       


    }
}
