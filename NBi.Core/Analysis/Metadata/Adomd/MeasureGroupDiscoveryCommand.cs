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

        public virtual MeasureGroupCollection List(IEnumerable<IFilter> filters)
        {
            var measureGroups = new MeasureGroupCollection();
            
            Inform("Investigating measure-groups");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measuregroup_dimensions WHERE DIMENSION_IS_VISIBLE{0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);
                // Traverse the response and 
                // read column 2, "CUBE_NAME"
                // read column 3, "MEASUREGROUP_NAME"
                while (rdr.Read())
                {
                    string perspectiveName = (string)rdr.GetValue(2);
                    if (!perspectiveName.StartsWith("$"))
                    {
                        // Get the column value
                        string name = (string)rdr.GetValue(3);
                        MeasureGroup mg;
                        measureGroups.AddOrIgnore(name);
                    }
                }
            }

            return measureGroups;
        }

        public virtual IEnumerable<IField> GetCaptions(IEnumerable<IFilter> filters)
        {
            var values = List(filters);
            return values.Values.ToArray();
        }

        public virtual string Build(IEnumerable<IFilter> filters)
        {
            var filterString = string.Empty;
            foreach (var filter in filters)
            {
                if (filter != null)
                    filterString += string.Format(" and {0}", Build((CaptionFilter)filter));
            }

            return filterString;
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
