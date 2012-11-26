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
            var hierarchies = new HierarchyCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                hierarchies.AddOrIgnore(row.UniqueName, row.Caption);

            return hierarchies;
        }

        internal new IEnumerable<HierarchyRow> Discover(IEnumerable<IFilter> filters)
        {
            Filters = filters;
            var hierarchies = new List<HierarchyRow>();

            Inform("Investigating hierarchies");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_hierarchies where 1=1 {0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);

                while (rdr.Read())
                {
                    var hieRow = HierarchyRow.Load(rdr);
                    if (hieRow != null)
                        hierarchies.Add(hieRow);
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
