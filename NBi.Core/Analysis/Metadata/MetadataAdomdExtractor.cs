using System;
using System.Collections.Generic;
using Microsoft.AnalysisServices.AdomdClient;
using System.Linq;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Metadata
{
    public class MetadataAdomdExtractor : IMetadataExtractor
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
  
        public CubeMetadata GetFullMetadata()
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Starting investigation ..."));

            GetPerspectives(Filter.Empty);
            GetDimensions(Filter.Empty);
            GetHierarchies(Filter.Empty);
            GetLevels(Filter.Empty);
            GetProperties(Filter.Empty);
            GetDimensionUsage(Filter.Empty);
            GetMeasures(Filter.Empty);

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Cube investigated"));

            return Metadata;
        }

        /// <summary>
        /// Retrieve partial metadata from an Olap cube. The zone of the cube investigated is delimited by the DiscoverCommand parameter
        /// </summary>
        /// <param name="command">limit the scope of the metadata's investigation</param>
        /// <returns>An enumration of fields</returns>
        public IEnumerable<IField> GetPartialMetadata(DiscoveryCommand command)
        {
            var filter = command.Filter;
            var depthController = command.Depth;
            Console.Out.WriteLine(filter);

            //Execute the discovery command
            //the depthController ensure we don't go too through in the discovery
            if (depthController.Perspectives)
                GetPerspectives(filter);
            if (depthController.Dimensions)
                GetDimensions(filter);
            if (depthController.Hierarchies)
                GetHierarchies(filter);
            if (depthController.Levels)
                GetLevels(filter);
            if (depthController.MeasureGroups)
                GetDimensionUsage(filter);
            if (depthController.Measures)
                GetMeasures(filter);

            //Return result of the discovery command

            //perspectives
            if(command.Target==DiscoveryTarget.Perspectives)
                return Metadata.Perspectives.Values.AsEnumerable<IField>();
            if (Metadata.Perspectives.ContainsKey(filter.Perspective))
            {
                //dimensions and measure-groups
                var persp = Metadata.Perspectives[filter.Perspective];
                if(command.Target==DiscoveryTarget.Dimensions)
                    return persp.Dimensions.Values.AsEnumerable<IField>();
                //if(command.Target==DiscoveryTarget.MeasureGroups)
                //    return persp.MeasureGroups.Values.AsEnumerable<IField>();
                
                //hierarchies & levels
                if (depthController.Dimensions)
                {
                    if (persp.Dimensions.ContainsKey(filter.DimensionUniqueName))
                    {
                        var dim = persp.Dimensions[filter.DimensionUniqueName];
                        if(command.Target==DiscoveryTarget.Hierarchies)
                            return dim.Hierarchies.Values.AsEnumerable<IField>();

                        if (dim.Hierarchies.ContainsKey(filter.HierarchyUniqueName))
                        {
                            var hie = dim.Hierarchies[filter.HierarchyUniqueName];
                            if(command.Target==DiscoveryTarget.Levels)
                                return hie.Levels.Values.AsEnumerable<IField>();
                        }
                        else
                            throw new MetadataNotFoundException("The hierarchy named '{0}' doesn't exist", filter.HierarchyUniqueName);
                    }
                    else
                        throw new MetadataNotFoundException("The dimension named '{0}' doesn't exist", filter.DimensionUniqueName);
                }
                
                //Measures
                if (depthController.MeasureGroups)
                {
                    if (persp.MeasureGroups.ContainsKey(filter.MeasureGroupName))
                    {
                        var mg = persp.MeasureGroups[filter.MeasureGroupName];
                        if(command.Target==DiscoveryTarget.Measures)
                            return mg.Measures.Values.AsEnumerable<IField>();
                    }
                    else
                        throw new MetadataNotFoundException("The measure-group named '{0}' doesn't exist", filter.MeasureGroupName);
                }
            }
            else
                throw new MetadataNotFoundException("The perspective named '{0}' doesn't exist", filter.Perspective);

            throw new Exception("Unhandled case for partial metadata extraction!");
        }

        internal void GetPerspectives(Filter filter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating perspectives"));
            using (var cmd = CreateCommand())
            {
                var whereClause = string.IsNullOrEmpty(filter.Perspective) ? string.Empty : string.Format(" and CUBE_NAME='{0}'", filter.Perspective);
                cmd.CommandText = string.Format("select * from $system.mdschema_dimensions where 1=1{0}", whereClause);
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

        internal void GetDimensions(Filter filter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating dimensions"));

            using (var cmd = CreateCommand())
            {
                var whereClause = string.IsNullOrEmpty(filter.Perspective) ? string.Empty : string.Format(" and CUBE_NAME='{0}'", filter.Perspective);
                whereClause += string.IsNullOrEmpty(filter.DimensionUniqueName) ? string.Empty : string.Format(" and [DIMENSION_UNIQUE_NAME]='{0}'", filter.DimensionUniqueName);
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

        internal void GetHierarchies(Filter filter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating hierarchies"));

            using (var cmd = CreateCommand())
            {
                var whereClause = string.IsNullOrEmpty(filter.Perspective) ? string.Empty : string.Format(" and CUBE_NAME='{0}'", filter.Perspective);
                whereClause += string.IsNullOrEmpty(filter.DimensionUniqueName) ? string.Empty : string.Format(" and [DIMENSION_UNIQUE_NAME]='{0}'", filter.DimensionUniqueName);
                whereClause += string.IsNullOrEmpty(filter.HierarchyUniqueName) ? string.Empty : string.Format(" and [HIERARCHY_UNIQUE_NAME]='{0}'", filter.HierarchyUniqueName);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_hierarchies where 1=1 {0}", whereClause);
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

        internal void GetLevels(Filter filter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating levels"));

            using (var cmd = CreateCommand())
            {
                var whereClause = string.IsNullOrEmpty(filter.Perspective) ? string.Empty : string.Format(" and CUBE_NAME='{0}'", filter.Perspective);
                whereClause += string.IsNullOrEmpty(filter.DimensionUniqueName) ? string.Empty : string.Format(" and [DIMENSION_UNIQUE_NAME]='{0}'", filter.DimensionUniqueName);
                whereClause += string.IsNullOrEmpty(filter.HierarchyUniqueName) ? string.Empty : string.Format(" and [HIERARCHY_UNIQUE_NAME]='{0}'", filter.HierarchyUniqueName);
                whereClause += string.IsNullOrEmpty(filter.LevelUniqueName) ? string.Empty : string.Format(" and [LEVEL_UNIQUE_NAME]='{0}'", filter.LevelUniqueName);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_levels where 1=1 {0}", whereClause);
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

        internal void GetProperties(Filter filter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating properties"));

            using (var cmd = CreateCommand())
            {
                var whereClause = string.IsNullOrEmpty(filter.Perspective) ? string.Empty : string.Format(" and CUBE_NAME='{0}'", filter.Perspective);
                whereClause += string.IsNullOrEmpty(filter.DimensionUniqueName) ? string.Empty : string.Format(" and [DIMENSION_UNIQUE_NAME]='{0}'", filter.DimensionUniqueName);
                whereClause += string.IsNullOrEmpty(filter.HierarchyUniqueName) ? string.Empty : string.Format(" and [HIERARCHY_UNIQUE_NAME]='{0}'", filter.HierarchyUniqueName);
                whereClause += string.IsNullOrEmpty(filter.LevelUniqueName) ? string.Empty : string.Format(" and [LEVEL_UNIQUE_NAME]='{0}'", filter.LevelUniqueName);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_properties where 1=1 {0}", whereClause);
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

        internal void GetDimensionUsage(Filter filter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating measure groups and dimensions usage"));

            using (var cmd = CreateCommand())
            {
                var whereClause = string.IsNullOrEmpty(filter.Perspective) ? string.Empty : string.Format(" and CUBE_NAME='{0}'", filter.Perspective);
                whereClause += string.IsNullOrEmpty(filter.MeasureGroupName) ? string.Empty : string.Format(" and [MEASUREGROUP_NAME]='{0}'", filter.MeasureGroupName);
                whereClause += string.IsNullOrEmpty(filter.DimensionUniqueName) ? string.Empty : string.Format(" and [DIMENSION_UNIQUE_NAME]='{0}'", filter.DimensionUniqueName);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measuregroup_dimensions WHERE DIMENSION_IS_VISIBLE{0}", whereClause);
                Console.Out.WriteLine(cmd.CommandText);
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

        internal void GetMeasures(Filter filter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating measures"));

            using (var cmd = CreateCommand())
            {
                var whereClause = string.IsNullOrEmpty(filter.Perspective) ? string.Empty : string.Format(" and CUBE_NAME='{0}'", filter.Perspective);
                whereClause += string.IsNullOrEmpty(filter.MeasureGroupName) ? string.Empty : string.Format(" and [MEASUREGROUP_NAME]='{0}'", filter.MeasureGroupName);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measures WHERE MEASURE_IS_VISIBLE and LEN(MEASUREGROUP_NAME)>0{0}", whereClause);
                Console.Out.WriteLine(cmd.CommandText);
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
                        string displayFolder = (string)rdr.GetValue(19);
                        mg.Measures.Add(uniqueName, caption, displayFolder);
                    }
                }
            }
        }
    }
}
