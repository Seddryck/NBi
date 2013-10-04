using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class HierarchyDiscoveryCommand : DimensionDiscoveryCommand
    {
        public ICollection<PostCommandFilter> PostCommandFilters { get; private set; }

        public HierarchyDiscoveryCommand(string connectionString)
            : base(connectionString)
        {
            PostCommandFilters = new List<PostCommandFilter>();
        }

        public new HierarchyCollection List(IEnumerable<IFilter> filters)
        {
            var hierarchies = new HierarchyCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                hierarchies.AddOrIgnore(row.UniqueName, row.Caption, row.DisplayFolder);

            return hierarchies;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
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
                    if (PostFilter(hieRow)) 
                        if (hieRow != null)
                            hierarchies.Add(hieRow);
                }
            }

            return hierarchies;
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

            if (filter.Target == DiscoveryTarget.Hierarchies)
            {
                    var dimFilter = FindFilter(DiscoveryTarget.Dimensions);
                    return string.Format("[HIERARCHY_UNIQUE_NAME]='[{0}].[{1}]'", dimFilter.Value, filter.Value);
            }

            if (filter.Target == DiscoveryTarget.DisplayFolders)
                PostCommandFilters.Add(new DisplayFolderFilter(filter.Value));
            
            return string.Empty;
        }

        protected CaptionFilter FindFilter(DiscoveryTarget target)
        {
            var filter = Filters.First(f => f is CaptionFilter && ((CaptionFilter)f).Target == target);
            return (CaptionFilter)filter;
        }

        /// <summary>
        /// PostFilter method is specifically build to enable filter aftare the execution of the command.
        /// For some attributes such as Display-Folder you cannot apply a filter in the command, in this case the filter is applied on the resultset
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected bool PostFilter(HierarchyRow row)
        {
            foreach (var postCommandFilter in PostCommandFilters)
                if (!postCommandFilter.Evaluate(row))
                    return false;

            return true;
        }
    }
}
