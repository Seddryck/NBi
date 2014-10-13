using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class DimensionLinkedToDiscoveryCommand : LinkedToDiscoveryCommand
    {
        public DimensionLinkedToDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }

        public virtual MeasureGroupCollection List(IEnumerable<IFilter> filters)
        {
            var measureGroups = new MeasureGroupCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
            {
                measureGroups.AddOrIgnore(row.Name);
            }

            return measureGroups;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal IEnumerable<MeasureGroupRow> Discover(IEnumerable<IFilter> filters)
        {
            var measureGroups = new List<MeasureGroupRow>();
            
            Inform("Investigating links to a dimension");

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

        public override IEnumerable<IField> Execute()
        {
            var values = List(Filters);
            return values.Values.ToArray();
        }
    }
}
