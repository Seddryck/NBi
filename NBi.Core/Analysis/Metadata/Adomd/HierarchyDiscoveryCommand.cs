using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class HierarchyDiscoveryCommand : DimensionDiscoveryCommand
    {
        protected IEnumerable<IFilter> Filters;

        public HierarchyDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }

        public new HierarchyCollection List(IEnumerable<IFilter> filters)
        {
            Filters = filters;
            var hierarchies = new HierarchyCollection();

            Inform("Investigating hierarchies");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_hierarchies where 1=1 {0}", adomdFiltering);
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
                        //string dimensionUniqueName = (string)rdr.GetValue(3);
                        if (true) //Needed to avoid dimension [Measure] previously filtered
                        //Metadata.Perspectives[perspectiveName].Dimensions.ContainsKey(dimensionUniqueName)
                        {
                            string uniqueName = (string)rdr.GetValue(5);
                            string caption = (string)rdr.GetValue(7);
                            hierarchies.AddOrIgnore(uniqueName, caption);
                        }
                    }
                }
            }

            return hierarchies;
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

            if (filter.Target == DiscoveryTarget.Hierarchies)
            {
                    var dimFilter = FindFilter(DiscoveryTarget.Dimensions);
                    return string.Format("[HIERARCHY_UNIQUE_NAME]='[{0}].[{1}]'", dimFilter.Value, filter.Value);
            }
            
            return string.Empty;
        }

        protected CaptionFilter FindFilter(DiscoveryTarget target)
        {
            var filter = Filters.First(f => f is CaptionFilter && ((CaptionFilter)f).Target == target);
            return (CaptionFilter)filter;
        }
    }
}
