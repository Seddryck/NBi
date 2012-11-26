using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class MeasureGroupDiscoveryCommand : PerspectiveDiscoveryCommand
    {
        public MeasureGroupDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }

        public new virtual MeasureGroupCollection List(IEnumerable<IFilter> filters)
        {
            var measureGroups = new MeasureGroupCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
            {
                measureGroups.AddOrIgnore(row.Name);
            }

            return measureGroups;
        }

        internal new IEnumerable<MeasureGroupRow> Discover(IEnumerable<IFilter> filters)
        {
            var measureGroups = new List<MeasureGroupRow>();
            
            Inform("Investigating measure-groups");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measuregroup_dimensions WHERE DIMENSION_IS_VISIBLE{0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);

                while (rdr.Read())
                {
                    var row = MeasureGroupRow.Load(rdr);
                    if (row != null)
                        measureGroups.Add(row);
                }
            }

            return measureGroups;
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

            if (filter.Target == DiscoveryTarget.MeasureGroups)
                return string.Format("[MEASUREGROUP_NAME]='{0}'", filter.Value);

            return string.Empty;
        }
    }
}
