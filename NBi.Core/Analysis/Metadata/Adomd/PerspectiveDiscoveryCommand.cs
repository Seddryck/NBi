using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class PerspectiveDiscoveryCommand : AdomdDiscoveryCommand
    {
        public PerspectiveDiscoveryCommand(string connectionString) : base(connectionString)
        {

        }

        public PerspectiveCollection List(IEnumerable<IFilter> filters)
        {
            var perspectives = new PerspectiveCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                perspectives.AddOrIgnore(row.Name);

            return perspectives;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal IEnumerable<PerspectiveRow> Discover(IEnumerable<IFilter> filters)
        {
            var perspectives = new List<PerspectiveRow>();
            
            Inform("Investigating perspectives");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("select * from $system.mdschema_dimensions where 1=1{0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);
                // Traverse the response and 
                // read column 2, "CUBE_NAME"
                while (rdr.Read())
                {
                    var row = PerspectiveRow.Load(rdr);
                    if (row != null)
                        perspectives.Add(row);
                }
            }

            return perspectives;
        }


        public override IEnumerable<IField> Execute()
        {
            var values = List(Filters);
            return values.Values.ToArray();
        }

        public virtual string Build(IEnumerable<IFilter> filters)
        {
            Filters = filters;
            if (filters == null)
                return string.Empty;

            var filterString = string.Empty;
            foreach (var filter in filters)
            {
                if (filter != null)
                {
                    var newFilter = Build((CaptionFilter)filter);
                    //We need to check if the filter will not return an null or empty string because postCommandFilters will return a null string
                    //If we don't test we still add to the filterString and have issues
                    if (!string.IsNullOrEmpty(newFilter))
                        filterString += string.Format(" and {0}", newFilter);
                }
            }

            return filterString;
        }

        protected override string Build(CaptionFilter filter)
        {
            if (filter.Target==DiscoveryTarget.Perspectives)
                    return string.Format("CUBE_NAME='{0}'", filter.Value);

            return string.Empty;
        }
    }
}
