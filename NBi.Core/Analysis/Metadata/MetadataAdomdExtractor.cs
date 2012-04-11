using System.IO;
using System.Xml.Serialization;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataAdomdExtractor
    {
        public event ProgressStatusHandler ProgressStatusChanged;
        
        public string ConnectionString { get; private set; }
        protected CubeMetadata Metadata;

        public MetadataAdomdExtractor(string connectionString) 
        {
            ConnectionString = connectionString;
            Metadata = new CubeMetadata();
        }

        protected AdomdCommand CreateCommand()
        {
            var conn = new AdomdConnection();
            conn.ConnectionString = ConnectionString;
            conn.Open();

            var cmd = new AdomdCommand();
            cmd.Connection = conn;
            return cmd;
        }

        public CubeMetadata GetMetadata()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Starting investigating ..."));

            GetPerspectives();
            GetDimensions();
            GetHierarchies();
            GetDimensionUsage();
            GetMeasures();

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Cube investigated"));

            return Metadata;
        }

        protected void GetPerspectives()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating perspectives"));
            using (var cmd = CreateCommand())
            {
                string whereClause = "";
                //whereClause = string.Format(" and CUBE_NAME='{0}'", perspectiveName);
                cmd.CommandText = string.Format("select * from $system.mdschema_dimensions{0}", whereClause);
                AdomdDataReader rdr = cmd.ExecuteReader();
                // Traverse the response and 
                // read column 2, "CUBE_NAME"
                while (rdr.Read())
                {
                    string perspectiveName = (string)rdr.GetValue(2);
                    if (!perspectiveName.StartsWith("$"))
                    {
                        //Manage Perspectives 
                        Metadata.Perspectives.AddOrIgnore(perspectiveName);
                    }
                }
            }
        }

        protected void GetDimensions()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating dimensions"));

            using (var cmd = CreateCommand())
            {
                string whereClause = "";
                //whereClause = string.Format(" and CUBE_NAME='{0}'", perspectiveName);
                cmd.CommandText = string.Format("select * from $system.mdschema_dimensions where DIMENSION_IS_VISIBLE{0}", whereClause);
                AdomdDataReader rdr = cmd.ExecuteReader();
                // Traverse the response and 
                // read column 2, "CUBE_NAME"
                // read column 4, "DIMENSION_UNIQUE_NAME"
                // read column 6, "DIMENSION_CAPTION"
                // read column 8, "DIMENSION_TYPE"
                // read column 10, "DEFAULT HIERARCHY"
                while (rdr.Read())
                {
                    string perspectiveName = (string)rdr.GetValue(2);
                    if ((short)rdr.GetValue(8) != 2 && !perspectiveName.StartsWith("$"))
                    {
                        // Get the columns value
                        string uniqueName = (string)rdr.GetValue(4);
                        string caption = (string)rdr.GetValue(6);
                        string defaultHierarchy = (string)rdr.GetValue(10);
                        Metadata.Perspectives[perspectiveName].Dimensions.AddOrIgnore(uniqueName, caption, defaultHierarchy);
                    }
                }
            }
        }

        protected void GetHierarchies()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating hierarchies"));

            using (var cmd = CreateCommand())
            {

                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_hierarchies");
                AdomdDataReader rdr = cmd.ExecuteReader();

                // Traverse the response and 
                // read column 2, "CUBE_NAME"
                // read column 3, "DIMENSION_UNIQUE_NAME"
                // read column 21, "HIERARCHY_IS_VISIBLE"
                // read column 5, "HIERARCHY_UNIQUE_NAME"
                // read column 7, "HIERARCHY_CAPTION"
                while (rdr.Read())
                {
                    // Get the column value
                    string perspectiveName = (string)rdr.GetValue(2);
                    if (!perspectiveName.StartsWith("$") && (bool)rdr.GetValue(21))
                    {
                        string dimensionUniqueName = (string)rdr.GetValue(3);
                        if (Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)) //Needed to avoid dimension [Measure previously filtered]
                        {
                            var dim = Metadata.Perspectives[perspectiveName].Dimensions[dimensionUniqueName];
                            string uniqueName = (string)rdr.GetValue(5);
                            string caption = (string)rdr.GetValue(7);
                            dim.Hierarchies.AddOrIgnore(uniqueName, caption);
                        }
                    }
                }
            }
        }

        protected void GetDimensionUsage()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating measure groups and dimensions usage"));

            using (var cmd = CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measuregroup_dimensions WHERE DIMENSION_IS_VISIBLE");
                AdomdDataReader rdr = cmd.ExecuteReader();

                // Traverse the response and 
                // read column 2, "CUBE_NAME"
                // read column 3, "MEASUREGROUP_NAME"
                // read column 5, "DIMENSION_UNIQUE_NAME"
                while (rdr.Read())
                {
                    string perspectiveName = (string)rdr.GetValue(2);
                    if (!perspectiveName.StartsWith("$"))
                    {
                        // Get the column value
                        string name = (string)rdr.GetValue(3);
                        MeasureGroup mg;
                        if (Metadata.Perspectives[perspectiveName].MeasureGroups.ContainsKey(name))
                            mg = Metadata.Perspectives[perspectiveName].MeasureGroups[name];
                        else
                        {
                            mg = new MeasureGroup(name);
                            Metadata.Perspectives[perspectiveName].MeasureGroups.AddOrIgnore(name);
                        }

                        string dimensionUniqueName = (string)rdr.GetValue(5);
                        if (Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)) //Needed to avoid dimension [Measure previously filtered]
                            mg.LinkedDimensions.Add(Metadata.Perspectives[perspectiveName].Dimensions[dimensionUniqueName]);
                    }
                }
            }
        }

        protected void GetMeasures()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating measures"));

            using (var cmd = CreateCommand())
            {

                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measures WHERE MEASURE_IS_VISIBLE and LEN(MEASUREGROUP_NAME)>0");
                AdomdDataReader rdr = cmd.ExecuteReader();

                // Traverse the response and 
                // read column 2, "CUBE_NAME"
                // read column 4, "MEASURE_UNIQUE_NAME"
                // read column 5, "MEASURE_CAPTION"
                // read column 18, "MEASUREGROUP_NAME"

                while (rdr.Read())
                {
                    string perspectiveName = (string)rdr.GetValue(2);
                    if (!perspectiveName.StartsWith("$"))
                    {
                        // Get the column value
                        string nameMeasureGroup = (string)rdr.GetValue(18);
                        var mg = Metadata.Perspectives[perspectiveName].MeasureGroups[nameMeasureGroup];

                        string uniqueName = (string)rdr.GetValue(4);
                        string caption = (string)rdr.GetValue(5);
                        mg.Measures.Add(uniqueName, caption);
                    }
                }
            }
        }

    }
}
