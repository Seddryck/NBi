using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Analysis.Request
{
    public class BaseDiscoveryRequest
    {
        public string ConnectionString { get; set; }
        protected IDictionary<DiscoveryTarget, IFilter> Filters { get; set; }
        internal IList<Validation> Validations { get; set; }

        public BaseDiscoveryRequest()
        {
            Filters = new Dictionary<DiscoveryTarget, IFilter>();
        }

        protected void AddFilters(IEnumerable<IFilter> filters)
        {
            foreach (var filter in filters)
                SpecifyFilter(filter);
        }

        public void SpecifyFilter(IFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            
            if (Filters.ContainsKey(filter.Target))
                Filters[filter.Target] = filter;
            else
                Filters.Add(filter.Target, filter);
        }

        public IFilter GetFilter(DiscoveryTarget target)
        {
            if (Filters.ContainsKey(target))
                return Filters[target];
            else
                return null;
        }

        public IEnumerable<IFilter> GetAllFilters()
        {
            return Filters.Values.ToArray();
        }

        public virtual string Path
        {
            get
            {
                string path = string.Empty;

                var dim = GetFilter(DiscoveryTarget.Dimensions);
                if (dim == null)
                    return path;

                path = string.Format("[{0}]", dim.Value);

                var hie = GetFilter(DiscoveryTarget.Hierarchies);
                if (hie == null)
                    return path;

                path = string.Format("{0}.[{1}]", path, hie.Value);

                var lev = GetFilter(DiscoveryTarget.Levels);
                if (lev == null)
                    return path;

                path = string.Format("{0}.[{1}]", path, lev.Value);
                return path;
            }
        }

    }
}
