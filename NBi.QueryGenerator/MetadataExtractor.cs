using System.IO;
using System.Xml.Serialization;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.QueryGenerator
{
    public class MetadataExtractor
    {
        
        public string ConnectionString { get; private set; }
        public string Perspective { get; private set; }
        public Dimensions Dimensions { get; private set; }
        public MeasureGroups MeasureGroups { get; private set; }

        public MetadataExtractor(string connectionString, string perspective) 
        {
            ConnectionString = connectionString;
            Perspective = perspective;
            Dimensions = new Dimensions();
            MeasureGroups = new MeasureGroups();
        }

        public MetadataExtractor(MeasureGroups measureGroups, Dimensions dimensions)
        {
            Dimensions = dimensions;
            MeasureGroups = measureGroups;
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

        public void GetMetadata()
        {
            GetDimensions();
            foreach (var dim in Dimensions)
                GetHierarchies(dim.Key);
            GetDimensionUsage();
            GetMeasures();
        }

        public void GetDimensions()
        {
            using (var cmd = CreateCommand())
            {

                cmd.CommandText = string.Format("select * from $system.mdschema_dimensions where CUBE_NAME='{0}' and DIMENSION_IS_VISIBLE", Perspective);
                AdomdDataReader rdr = cmd.ExecuteReader();
                // Traverse the response and 
                // read column 4, "DIMENSION_UNIQUE_NAME"
                // read column 6, "DIMENSION_CAPTION"
                // read column 10, "DEFAULT HIERARCHY"
                while (rdr.Read())
                {
                    // Get the columns value
                    string uniqueName = (string)rdr.GetValue(4);
                    string caption = (string)rdr.GetValue(6);
                    string defaultHierarchy = (string)rdr.GetValue(10);
                    Dimensions.Add(uniqueName, caption, defaultHierarchy);
                }
            }
        }
  

        public void GetDimensionUsage()
        {
            using (var cmd = CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measuregroup_dimensions WHERE CUBE_NAME='{0}' and DIMENSION_IS_VISIBLE", Perspective);
                AdomdDataReader rdr = cmd.ExecuteReader();

                // Traverse the response and 
                // read column 3, "MEASUREGROUP_NAME"
                // read column 5, "DIMENSION_UNIQUE_NAME"
                while (rdr.Read())
                {
                    // Get the column value
                    string name = (string)rdr.GetValue(3);
                    MeasureGroup mg;
                    if (MeasureGroups.ContainsKey(name))
                        mg = MeasureGroups[name];
                    else
                    {
                        mg = new MeasureGroup(name);
                        MeasureGroups.Add(name);
                    }

                    string dimensionUniqueName = (string)rdr.GetValue(5);
                    mg.LinkedDimensions.Add(Dimensions[dimensionUniqueName]);
                }
            }
        }

        public void GetMeasures()
        {
            using (var cmd = CreateCommand())
            {

                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measures WHERE CUBE_NAME='{0}' and MEASURE_IS_VISIBLE and LEN(MEASUREGROUP_NAME)>0", Perspective);
                AdomdDataReader rdr = cmd.ExecuteReader();

                // Traverse the response and 
                // read column 4, "MEASURE_UNIQUE_NAME"
                // read column 5, "MEASURE_CAPTION"
                // read column 18, "MEASUREGROUP_NAME"

                while (rdr.Read())
                {
                    // Get the column value
                    string nameMeasureGroup = (string)rdr.GetValue(18);
                    var mg = MeasureGroups[nameMeasureGroup];

                    string uniqueName = (string)rdr.GetValue(4);
                    string caption = (string)rdr.GetValue(5);
                    mg.Measures.Add(uniqueName, caption);

                }
            }
        }

        public void GetHierarchies(string dimensionUniqueName)
        {
            using (var cmd = CreateCommand())
            {

                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_hierarchies WHERE CUBE_NAME='{0}' AND [DIMENSION_UNIQUE_NAME]='{1}'", Perspective, dimensionUniqueName);
                AdomdDataReader rdr = cmd.ExecuteReader();

                // Traverse the response and 
                // read column 21, "HIERARCHY_IS_VISIBLE"
                // read column 5, "HIERARCHY_UNIQUE_NAME"
                // read column 7, "HIERARCHY_CAPTION"

                var dim = Dimensions[dimensionUniqueName];
                while (rdr.Read())
                {
                    // Get the column value
                    if ((bool)rdr.GetValue(21))
                    {
                        string uniqueName = (string)rdr.GetValue(5);
                        string caption = (string)rdr.GetValue(7);
                        dim.Hierarchies.AddOrReplace(uniqueName, caption);
                    }
                }
            }
        }

        public void Perisist(string filename)
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(MeasureGroups));

            using (StreamWriter writer = new StreamWriter(filename))
            {
                // Use the Serialize method to store the object's state.
                serializer.Serialize(writer, MeasureGroups);
            }
        }

    }
}
