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
            var levels = new LevelCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                levels.AddOrIgnore(row.UniqueName, row.Caption, row.Number);

            return levels;
        }

        internal new IEnumerable<LevelRow> Discover(IEnumerable<IFilter> filters)
        {
            Filters = filters;
            var levels = new List<LevelRow>();

            Inform("Investigating levels");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_levels where 1=1 {0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);

                while (rdr.Read())
                {
                    var row = LevelRow.Load(rdr);
                    if (row != null)
                        levels.Add(row);
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
