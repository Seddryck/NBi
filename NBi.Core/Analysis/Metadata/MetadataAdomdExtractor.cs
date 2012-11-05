using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Analysis.Discovery;
using NBi.Core.Analysis.Metadata.Adomd;

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

            GetPerspectives(null);
            GetDimensions(null, null);
            GetHierarchies(null, null, null);
            GetLevels(null, null, null, null);
            GetProperties(null, null, null, null);
            GetDimensionUsage(null, null, null);
            GetMeasures(null, null);

            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Cube investigated"));

            return Metadata;
        }

        /// <summary>
        /// Retrieve partial metadata from an Olap cube. The zone of the cube investigated is delimited by the DiscoverCommand parameter
        /// </summary>
        /// <param name="command">limit the scope of the metadata's investigation</param>
        /// <returns>An enumration of fields</returns>
        public IEnumerable<IField> GetPartialMetadata(MetadataDiscoveryCommand command)
        {
            if (command.Target == DiscoveryTarget.Perspectives)
            {
                var cmd = new PerspectiveDiscoveryCommand(ConnectionString);
                return cmd.GetCaptions(command.GetAllFilters());
            }

            if (command.Target == DiscoveryTarget.Dimensions)
            {
                var cmd = new DimensionDiscoveryCommand(ConnectionString);
                return cmd.GetCaptions(command.GetAllFilters());
            }

            if (command.Target == DiscoveryTarget.Hierarchies)
            {
                var cmd = new HierarchyDiscoveryCommand(ConnectionString);
                return cmd.GetCaptions(command.GetAllFilters());
            }

            if (command.Target == DiscoveryTarget.Levels)
            {
                var cmd = new LevelDiscoveryCommand(ConnectionString);
                return cmd.GetCaptions(command.GetAllFilters());
            }

            if (command.Target == DiscoveryTarget.MeasureGroups)
            {
                var cmd = new MeasureGroupDiscoveryCommand(ConnectionString);
                return cmd.GetCaptions(command.GetAllFilters());
            }

            if (command.Target == DiscoveryTarget.Measures)
            {
                var cmd = new MeasureDiscoveryCommand(ConnectionString);
                return cmd.GetCaptions(command.GetAllFilters());
            }

            if (command.GetFilter(DiscoveryTarget.Dimensions) != null || command.Target == DiscoveryTarget.Dimensions)
                GetDimensions(
                    command.GetFilter(DiscoveryTarget.Perspectives),
                    command.GetFilter(DiscoveryTarget.Dimensions)
                    );

            if (command.GetFilter(DiscoveryTarget.Hierarchies) != null || command.Target == DiscoveryTarget.Hierarchies)
                GetHierarchies(
                    command.GetFilter(DiscoveryTarget.Perspectives),
                    command.GetFilter(DiscoveryTarget.Dimensions),
                    command.GetFilter(DiscoveryTarget.Hierarchies)
                    );

            if (command.GetFilter(DiscoveryTarget.Levels) != null || command.Target == DiscoveryTarget.Levels)
                GetLevels(
                    command.GetFilter(DiscoveryTarget.Perspectives),
                    command.GetFilter(DiscoveryTarget.Dimensions),
                    command.GetFilter(DiscoveryTarget.Hierarchies),
                    command.GetFilter(DiscoveryTarget.Levels)
                    );

            if (command.GetFilter(DiscoveryTarget.MeasureGroups) != null || command.Target == DiscoveryTarget.MeasureGroups)
                GetDimensionUsage(
                    command.GetFilter(DiscoveryTarget.Perspectives),
                    command.GetFilter(DiscoveryTarget.MeasureGroups),
                    command.GetFilter(DiscoveryTarget.Dimensions)
                    );

            if (command.GetFilter(DiscoveryTarget.Measures) != null || command.Target == DiscoveryTarget.Measures)
                GetMeasures(
                    command.GetFilter(DiscoveryTarget.Perspectives),
                    command.GetFilter(DiscoveryTarget.MeasureGroups)
                    );

            //Return result of the discovery command

            //perspectives
            if(command.Target==DiscoveryTarget.Perspectives)
                return Metadata.Perspectives.Values.AsEnumerable<IField>();

            var perspectiveFilterValue = command.GetFilter(DiscoveryTarget.Perspectives).Value;
            if (Metadata.Perspectives.ContainsKey(perspectiveFilterValue))
            {
                //dimensions and measure-groups
                var persp = Metadata.Perspectives[perspectiveFilterValue];
                if(command.Target==DiscoveryTarget.Dimensions)
                    return persp.Dimensions.Values.AsEnumerable<IField>();
                if(command.Target==DiscoveryTarget.MeasureGroups)
                    return persp.MeasureGroups.Values.AsEnumerable<IField>();
                
                //hierarchies & levels
                if (command.Target == DiscoveryTarget.Hierarchies || command.Target == DiscoveryTarget.Levels)
                {
                    var dimensionFilterValue = command.GetFilter(DiscoveryTarget.Dimensions).Value;
                    if (persp.Dimensions.ContainsKey(string.Format("[{0}]", dimensionFilterValue)))
                    {
                        var dim = persp.Dimensions[string.Format("[{0}]", dimensionFilterValue)];
                        if(command.Target==DiscoveryTarget.Hierarchies)
                            return dim.Hierarchies.Values.AsEnumerable<IField>();

                        var hierarchyFilterValue = command.GetFilter(DiscoveryTarget.Hierarchies).Value;
                        if (dim.Hierarchies.ContainsKey(string.Format("[{0}].[{1}]", dimensionFilterValue, hierarchyFilterValue)))
                        {
                            var hie = dim.Hierarchies[string.Format("[{0}].[{1}]", dimensionFilterValue, hierarchyFilterValue)];
                            if(command.Target==DiscoveryTarget.Levels)
                                return hie.Levels.Values.AsEnumerable<IField>();
                        }
                        else
                            throw new MetadataNotFoundException("The hierarchy named '{0}' doesn't exist", hierarchyFilterValue);
                    }
                    else
                        throw new MetadataNotFoundException("The dimension named '{0}' doesn't exist", dimensionFilterValue);
                }
                
                //Measures
                if (command.Target == DiscoveryTarget.Measures)
                {
                    if (command.GetFilter(DiscoveryTarget.Dimensions) != null)
                    {
                        var measureGroupFilterValue = command.GetFilter(DiscoveryTarget.MeasureGroups).Value;
                        if (persp.MeasureGroups.ContainsKey(measureGroupFilterValue))
                        {
                            var mg = persp.MeasureGroups[measureGroupFilterValue];
                            if (command.Target == DiscoveryTarget.Measures)
                                return mg.Measures.Values.AsEnumerable<IField>();
                        }
                        else
                            throw new MetadataNotFoundException("The measure-group named '{0}' doesn't exist", measureGroupFilterValue);
                    }
                    else
                    {
                        var measures = new List<IField>();
                        foreach (var mg in persp.MeasureGroups)
                                measures.AddRange(mg.Value.Measures.Values.AsEnumerable<IField>());
                        return measures;
                    }
                }
            }
            else
                throw new MetadataNotFoundException("The perspective named '{0}' doesn't exist", perspectiveFilterValue);

            throw new Exception("Unhandled case for partial metadata extraction!");
        }

        internal void GetPerspectives(IFilter filter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating perspectives"));
            using (var cmd = CreateCommand())
            {
                var whereClause = filter!=null && filter.GetType() == typeof(CaptionFilter) ? string.Format(" and CUBE_NAME='{0}'", ((CaptionFilter)filter).Value) : string.Empty;
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

        internal void GetDimensions(IFilter perspective, IFilter dimension)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating dimensions"));

            using (var cmd = CreateCommand())
            {
                var whereClause = perspective.GetType() == typeof(CaptionFilter) ? string.Format(" and CUBE_NAME='{0}'", ((CaptionFilter)perspective).Value) : string.Empty;
                whereClause += dimension != null && dimension.GetType() == typeof(CaptionFilter) ? string.Format(" and [DIMENSION_UNIQUE_NAME]='[{0}]'", ((CaptionFilter)dimension).Value) : string.Empty;
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

        internal void GetHierarchies(IFilter perspective, IFilter dimension, IFilter hierarchy)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating hierarchies"));

            using (var cmd = CreateCommand())
            {
                var whereClause = perspective.GetType() == typeof(CaptionFilter) ? string.Format(" and CUBE_NAME='{0}'", ((CaptionFilter)perspective).Value) : string.Empty;
                whereClause += dimension.GetType() == typeof(CaptionFilter) ? string.Format(" and [DIMENSION_UNIQUE_NAME]='[{0}]'", ((CaptionFilter)dimension).Value) : string.Empty;
                whereClause += hierarchy != null && hierarchy.GetType() == typeof(CaptionFilter) ? string.Format(" and [HIERARCHY_UNIQUE_NAME]='[{0}].[{1}]'", ((CaptionFilter)dimension).Value, ((CaptionFilter)hierarchy).Value) : string.Empty;
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

        internal void GetLevels(IFilter perspectiveFilter, IFilter dimensionFilter, IFilter hierarchyFilter, IFilter levelFilter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating levels"));

            using (var cmd = CreateCommand())
            {
                var whereClause = perspectiveFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and CUBE_NAME='{0}'", ((CaptionFilter)perspectiveFilter).Value) : string.Empty;
                whereClause += dimensionFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [DIMENSION_UNIQUE_NAME]='[{0}]'", ((CaptionFilter)dimensionFilter).Value) : string.Empty;
                whereClause += hierarchyFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [HIERARCHY_UNIQUE_NAME]='[{0}].[{1}]'", ((CaptionFilter)dimensionFilter).Value, ((CaptionFilter)hierarchyFilter).Value) : string.Empty;
                whereClause += levelFilter != null && levelFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [LEVEL_UNIQUE_NAME]='[{0}].[{1}].[{2}]'", ((CaptionFilter)dimensionFilter).Value, ((CaptionFilter)hierarchyFilter).Value, ((CaptionFilter)levelFilter).Value) : string.Empty;
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

        internal void GetProperties(IFilter perspectiveFilter, IFilter dimensionFilter, IFilter hierarchyFilter, IFilter levelFilter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating properties"));

            using (var cmd = CreateCommand())
            {
                var whereClause = perspectiveFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and CUBE_NAME='{0}'", ((CaptionFilter)perspectiveFilter).Value) : string.Empty;
                whereClause += dimensionFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [DIMENSION_UNIQUE_NAME]='[{0}]'", ((CaptionFilter)dimensionFilter).Value) : string.Empty;
                whereClause += hierarchyFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [HIERARCHY_UNIQUE_NAME]='[{0}]'", ((CaptionFilter)hierarchyFilter).Value) : string.Empty;
                whereClause += levelFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [LEVEL_UNIQUE_NAME]='[{0}]'", ((CaptionFilter)levelFilter).Value) : string.Empty;
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

        internal void GetDimensionUsage(IFilter perspectiveFilter, IFilter measureGroupFilter, IFilter dimensionFilter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating measure groups and dimensions usage"));

            using (var cmd = CreateCommand())
            {
                var whereClause = perspectiveFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and CUBE_NAME='{0}'", ((CaptionFilter)perspectiveFilter).Value) : string.Empty;
                whereClause += dimensionFilter != null && dimensionFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [DIMENSION_UNIQUE_NAME]='[{0}]'", ((CaptionFilter)dimensionFilter).Value) : string.Empty;
                whereClause += measureGroupFilter != null && measureGroupFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [MEASUREGROUP_NAME]='{0}'", ((CaptionFilter)measureGroupFilter).Value) : string.Empty;
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

        internal void GetMeasures(IFilter perspectiveFilter, IFilter measureGroupFilter)
        {
            if (ProgressStatusChanged != null)
                ProgressStatusChanged(this, new ProgressStatusEventArgs("Investigating measures"));

            using (var cmd = CreateCommand())
            {
                var whereClause = perspectiveFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and CUBE_NAME='{0}'", ((CaptionFilter)perspectiveFilter).Value) : string.Empty;
                whereClause += measureGroupFilter != null && measureGroupFilter.GetType() == typeof(CaptionFilter) ? string.Format(" and [MEASUREGROUP_NAME]='{0}'", ((CaptionFilter)measureGroupFilter).Value) : string.Empty;
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
