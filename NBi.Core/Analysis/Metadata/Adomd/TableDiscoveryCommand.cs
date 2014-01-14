using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class TableDiscoveryCommand : AdomdDiscoveryCommand
    {
        public TableDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }

        public TableCollection List(IEnumerable<IFilter> filters)
        {
            var tables = new TableCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                tables.AddOrIgnore(row.Name);

            return tables;
        }

        internal IEnumerable<TableRow> Discover(IEnumerable<IFilter> filters)
        {
            var tables = new List<TableRow>();

            Inform("Investigating tables");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("select * from $system.dbschema_tables where 1=1{0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);

                while (rdr.Read())
                {
                    var row = TableRow.Load(rdr);
                    if (row != null)
                        tables.Add(row);
                }
            }

            return tables;
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
            if (filter.Target == DiscoveryTarget.Perspectives)
                return string.Format("[TABLE_SCHEMA]='{0}'", filter.Value);

            return string.Empty;
        }
    }
}
