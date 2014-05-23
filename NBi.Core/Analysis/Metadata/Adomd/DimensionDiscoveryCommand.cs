using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class DimensionDiscoveryCommand : PerspectiveDiscoveryCommand
    {
        public DimensionDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }
        
        public new DimensionCollection List(IEnumerable<IFilter> filters)
        {
            var dimensions = new DimensionCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                dimensions.AddOrIgnore(row.UniqueName, row.Caption);

            return dimensions;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal new IEnumerable<DimensionRow> Discover(IEnumerable<IFilter> filters)
        {
            var dimensions = new List<DimensionRow>();
            
            Inform("Investigating dimensions");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("select * from $system.mdschema_dimensions where DIMENSION_IS_VISIBLE{0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);
                // Traverse the response and 
                
                while (rdr.Read())
                {
                    var dimRow = DimensionRow.Load(rdr);
                    if (dimRow != null)
                        dimensions.Add(dimRow);
                }
            }

            return dimensions;
        }

        public override IEnumerable<IField> Execute()
        {
            var values = List(Filters);
            return values.Values.ToArray();
        }
        
        protected override string Build(CaptionFilter filter)
        {
            var str = base.Build(filter);
            if (!String.IsNullOrEmpty(str))
                return str;

            if (filter.Target == DiscoveryTarget.Dimensions)
                    return string.Format("[DIMENSION_UNIQUE_NAME]='[{0}]'", filter.Value);

            return string.Empty;
        }
    }
}
