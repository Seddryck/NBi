using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class LevelDiscoveryCommand : HierarchyDiscoveryCommand
    {
        public LevelDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }

        public new LevelCollection List(IEnumerable<IFilter> filters)
        {
            Filters = filters;
            var levels = new LevelCollection();

            Inform("Investigating levels");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_levels where 1=1 {0}", adomdFiltering);
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
                        if (true) //Needed to avoid dimension [Measure] previously filtered
                        //Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)
                        {
                            string uniqueName = (string)rdr.GetValue(6);
                            string caption = (string)rdr.GetValue(8);
                            int number = Convert.ToInt32((uint)rdr.GetValue(9));
                            levels.AddOrIgnore(uniqueName, caption, number);
                        }
                    }
                }
            }

            return levels;
        }

        public override IEnumerable<IField> GetCaptions(IEnumerable<IFilter> filters)
        {
            var values = List(filters);
            return values.Values.ToArray();
        }


        protected override string Build(CaptionFilter filter)
        {
            var str = base.Build(filter);
            if (!String.IsNullOrEmpty(str))
                return str;

            if (filter.Target == DiscoveryTarget.Levels)
            {
                var dimFilter = FindFilter(DiscoveryTarget.Dimensions);
                var hieFilter = FindFilter(DiscoveryTarget.Hierarchies);
                return string.Format("[LEVEL_UNIQUE_NAME]='[{0}].[{1}].[{2}]'", dimFilter.Value, hieFilter.Value, filter.Value);
            }

            return string.Empty;
        }

    }
}
