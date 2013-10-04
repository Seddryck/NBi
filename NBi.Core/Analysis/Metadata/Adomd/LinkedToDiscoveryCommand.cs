using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Request;

namespace NBi.Core.Analysis.Metadata.Adomd
{
    internal abstract class LinkedToDiscoveryCommand : AdomdDiscoveryCommand
    {
        public LinkedToDiscoveryCommand(string connectionString)
            : base(connectionString)
        {

        }

        //public abstract IEnumerable<IField> List(IEnumerable<IFilter> filters);

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
                return string.Format("[CUBE_NAME]='{0}'", filter.Value);
            if (filter.Target == DiscoveryTarget.MeasureGroups)
                return string.Format("[MEASUREGROUP_NAME]='{0}'", filter.Value);
            if (filter.Target == DiscoveryTarget.Dimensions)
                return string.Format("[DIMENSION_UNIQUE_NAME]='[{0}]'", filter.Value);

            return string.Empty;
        }

    }
}
