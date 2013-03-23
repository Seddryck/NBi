using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class MeasureGroupLinkedToDiscoveryCommand : LinkedToDiscoveryCommand
    {
        public MeasureGroupLinkedToDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }

        public virtual DimensionCollection List(IEnumerable<IFilter> filters)
        {
            var dimensions = new DimensionCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
            {
                dimensions.AddOrIgnore(row.UniqueName, row.Caption);
            }

            return dimensions;
        }

        internal IEnumerable<DimensionRow> Discover(IEnumerable<IFilter> filters)
        {
            var dimensions = new List<DimensionRow>();
            
            Inform("Investigating links to a measure-group");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measuregroup_dimensions WHERE DIMENSION_IS_VISIBLE{0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);

                while (rdr.Read())
                {
                    var row = DimensionRow.LoadLinkedTo(rdr);
                    if (row != null)
                        dimensions.Add(row);
                }
            }

            return dimensions;
        }

        public override IEnumerable<IField> Execute()
        {
            var values = List(Filters);
            return values.Values.ToArray();
        }
    }
}
