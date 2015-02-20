using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class SetDiscoveryCommand : PerspectiveDiscoveryCommand
    {
        public ICollection<PostCommandFilter> PostCommandFilters { get; private set; }

        public SetDiscoveryCommand(string connectionString)
            : base(connectionString)
        {
            PostCommandFilters = new List<PostCommandFilter>();
        }

        public new SetCollection List(IEnumerable<IFilter> filters)
        {
            var sets = new SetCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                sets.AddOrIgnore(row.UniqueName, row.Caption);

            return sets;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal new IEnumerable<SetRow> Discover(IEnumerable<IFilter> filters)
        {
            var sets = new List<SetRow>();

            Inform("Investigating sets");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("select * from $system.mdschema_sets where [scope]=1{0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);
                // Traverse the response and 

                while (rdr.Read())
                {
                    var setRow = SetRow.Load(rdr);
                    if (PostFilter(setRow))
                        if (setRow != null)
                            sets.Add(setRow);
                }
            }

            return sets;
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

            if (filter.Target == DiscoveryTarget.Sets)
            {
                return string.Format("[SET_NAME]='{0}'", filter.Value);
            }

            if (filter.Target == DiscoveryTarget.DisplayFolders)
                PostCommandFilters.Add(new DisplayFolderFilter(filter.Value));

            return string.Empty;
        }

        /// <summary>
        /// PostFilter method is specifically build to enable filter aftare the execution of the command.
        /// For some attributes such as Display-Folder you cannot apply a filter in the command, in this case the filter is applied on the resultset
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected bool PostFilter(SetRow row)
        {
            foreach (var postCommandFilter in PostCommandFilters)
                if (!postCommandFilter.Evaluate(row))
                    return false;

            return true;
        }
    }
}
