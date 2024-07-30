using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Analysis.Request
{
    public class BaseDiscoveryRequest
    {
        public string ConnectionString { get; }
        protected IDictionary<DiscoveryTarget, IFilter> Filters { get; set; }
        internal IList<Validation> Validations { get; set; } = [];

        public BaseDiscoveryRequest(string connectionString)
            => (ConnectionString, Filters) = (connectionString, new Dictionary<DiscoveryTarget, IFilter>());

        protected void AddFilters(IEnumerable<IFilter> filters)
        {
            foreach (var filter in filters)
                SpecifyFilter(filter);
        }

        public void SpecifyFilter(IFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            
            if (Filters.ContainsKey(filter.Target))
                Filters[filter.Target] = filter;
            else
                Filters.Add(filter.Target, filter);
        }

        public IFilter? GetFilter(DiscoveryTarget target)
            => Filters.TryGetValue(target, out var value)
                ? value
                : null;

        public IEnumerable<IFilter> GetAllFilters()
            => [.. Filters.Values];

        public virtual string Path
        {
            get
            {
                string path = string.Empty;

                var dim = GetFilter(DiscoveryTarget.Dimensions);
                if (dim == null)
                    return path;

                path = $"[{dim.Value}]";

                var hie = GetFilter(DiscoveryTarget.Hierarchies);
                if (hie == null)
                    return path;

                path = $"{path}.[{hie.Value}]";

                var lev = GetFilter(DiscoveryTarget.Levels);
                if (lev == null)
                    return path;

                path = $"{path}.[{lev.Value}]";
                return path;
            }
        }

    }
}
