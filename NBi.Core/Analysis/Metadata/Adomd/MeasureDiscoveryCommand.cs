using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal class MeasureDiscoveryCommand : MeasureGroupDiscoveryCommand
    {
        public ICollection<PostCommandFilter> PostCommandFilters { get; private set; }

        public MeasureDiscoveryCommand(string connectionString)
            : base(connectionString)
        {
            PostCommandFilters = new List<PostCommandFilter>();
        }

        public new virtual MeasureCollection List(IEnumerable<IFilter> filters)
        {
            var measures = new MeasureCollection();

            var rows = Discover(filters);
            foreach (var row in rows)
                measures.Add(row.UniqueName, row.Caption, row.DisplayFolder);

            return measures;
        }

        internal new IEnumerable<MeasureRow> Discover(IEnumerable<IFilter> filters)
        {
            var measures = new List<MeasureRow>();
            
            Inform("Investigating measures");

            using (var cmd = CreateCommand())
            {
                var adomdFiltering = Build(filters);
                cmd.CommandText = string.Format("SELECT * FROM $system.mdschema_measures WHERE MEASURE_IS_VISIBLE and LEN(MEASUREGROUP_NAME)>0{0}", adomdFiltering);
                var rdr = ExecuteReader(cmd);

                
                while (rdr.Read())
                {
                    var row = MeasureRow.Load(rdr);
                    if (PostFilter(row))                    
                        if (row != null)
                            measures.Add(row);
                }
            }

            return measures;
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

            if (filter.Target == DiscoveryTarget.Measures)
                return string.Format("[MEASURE_UNIQUE_NAME]='[Measures].[{0}]'", filter.Value);

            if (filter.Target == DiscoveryTarget.DisplayFolders)
                PostCommandFilters.Add(new DisplayFolderFilter(filter.Value));

            return string.Empty;
        }

        protected bool PostFilter(MeasureRow row)
        {
            foreach (var postCommandFilter in PostCommandFilters)
                if (!postCommandFilter.Evaluate(row))
                    return false;

            return true;
        }
    }
}
