using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class PropertyDiscoveryCommand : LevelDiscoveryCommand
    {
        public PropertyDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }

        public new PropertyCollection List(IEnumerable<IFilter> filters)
        {
            var properties = new PropertyCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                properties.AddOrIgnore(row.UniqueName, row.Caption);

            return properties;
        }

        internal new IEnumerable<PropertyRow> Discover(IEnumerable<IFilter> filters)
        {
            Filters = filters;
            var properties = new List<PropertyRow>();

            Inform("Investigating properties");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_properties where 1=1 {0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);

                while (rdr.Read())
                {
                    var row = PropertyRow.Load(rdr);
                    if (row != null)
                        properties.Add(row);
                }
            }

            return properties;
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

            if (filter.Target == DiscoveryTarget.Properties)
            {
                return string.Format("[PROPERTY_CAPTION]='{0}'", filter.Value);
            }

            return string.Empty;
        }

    }
}
