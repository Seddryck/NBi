using System;
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
            try
            {
                conn.Open();
            }
            catch (AdomdConnectionException ex)
            {
                
                throw new ConnectionException(ex);
            }
            

            var cmd = new AdomdCommand();
            cmd.Connection = conn;
            return cmd;
        }

        protected AdomdDataReader ExecuteReader(AdomdCommand cmd)
        {
            AdomdDataReader rdr = null;
            try
            {
                rdr = cmd.ExecuteReader();
                return rdr;
            }
            catch (AdomdConnectionException ex)
            { throw new ConnectionException(ex); }
            catch (AdomdErrorResponseException ex)
            { throw new ConnectionException(ex); }
        }
  
        public CubeMetadata GetMetadata()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Starting investigation ..."));

            GetPerspectives();
            GetDimensions();
            GetHierarchies();
            GetLevels();
            GetProperties();
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
                var rdr = ExecuteReader(cmd);
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
                var rdr = ExecuteReader(cmd);
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
                        Metadata.Perspectives[perspectiveName].Dimensions.AddOrIgnore(uniqueName, caption);
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
                var rdr = ExecuteReader(cmd);

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
                        if (Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)) //Needed to avoid dimension [Measure] previously filtered
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

        protected void GetLevels()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating levels"));

            using (var cmd = CreateCommand())
            {

                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_levels");
                var rdr = ExecuteReader(cmd);

                // Traverse the response and 
                // read column 2, "CUBE_NAME"
                // read column 3, "DIMENSION_UNIQUE_NAME"
                // read column 4, "HIERARCHY_UNIQUE_NAME"
                // read column 6, "LEVEL_UNIQUE_NAME"
                // read column 8, "LEVEL_CAPTION"
                // read column 9, "LEVEL_NUMBER"
                // read column 15, "LEVEL_IS_VISIBLE"
                while (rdr.Read())
                {
                    // Get the column value
                    string perspectiveName = (string)rdr.GetValue(2);
                    if (!perspectiveName.StartsWith("$") && (bool)rdr.GetValue(15))
                    {
                        string dimensionUniqueName = (string)rdr.GetValue(3);
                        if (Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)) //Needed to avoid dimension [Measure] previously filtered
                        {
                            var dim = Metadata.Perspectives[perspectiveName].Dimensions[dimensionUniqueName];
                            string hierarchyUniqueName = (string)rdr.GetValue(4);
                            if (dim.Hierarchies.ContainsKey(hierarchyUniqueName))
                            {
                                var hierarchy = dim.Hierarchies[hierarchyUniqueName];
                                string uniqueName = (string)rdr.GetValue(6);
                                string caption = (string)rdr.GetValue(8);
                                int number = Convert.ToInt32((uint)rdr.GetValue(9));
                                hierarchy.Levels.AddOrIgnore(uniqueName, caption, number);
                            }
                        }
                    }
                }
            }
        }

        protected void GetProperties()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating properties"));

            using (var cmd = CreateCommand())
            {

                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_properties");
                var rdr = ExecuteReader(cmd);

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
                while (rdr.Read())
                {
                    // Get the column value
                    string perspectiveName = (string)rdr.GetValue(2);
                    if (!perspectiveName.StartsWith("$") && (bool)rdr.GetValue(23) && ((short)rdr.GetValue(7))==1)
                    {
                        string dimensionUniqueName = (string)rdr.GetValue(3);
                        if (Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)) //Needed to avoid dimension [Measure] previously filtered
                        {
                            var dim = Metadata.Perspectives[perspectiveName].Dimensions[dimensionUniqueName];
                            string hierarchyUniqueName = (string)rdr.GetValue(4);
                            if (dim.Hierarchies.ContainsKey(hierarchyUniqueName))
                            {
                                var hierarchy = dim.Hierarchies[hierarchyUniqueName];
                                string levelUniqueName = (string)rdr.GetValue(5);
                                if (hierarchy.Levels.ContainsKey(levelUniqueName))
                                {
                                    var level = hierarchy.Levels[levelUniqueName];
                                    string name = (string)rdr.GetValue(8);
                                    string uniqueName = string.Format("{0}.[{1}]", levelUniqueName, name);
                                    string caption = (string)rdr.GetValue(9);
                                    //TODO Add data type management
                                    level.Properties.AddOrIgnore(uniqueName, caption);
                                }
                            }
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
                var rdr = ExecuteReader(cmd);

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
                              Metadata.Perspectives[perspectiveName].MeasureGroups.AddOrIgnore(name);
  
                        string dimensionUniqueName = (string)rdr.GetValue(5);
                        if (Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)) //Needed to avoid dimension [Measure previously filtered]
                            Metadata.Perspectives[perspectiveName].MeasureGroups[name].LinkedDimensions.Add(Metadata.Perspectives[perspectiveName].Dimensions[dimensionUniqueName]);
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
                var rdr = ExecuteReader(cmd);

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
