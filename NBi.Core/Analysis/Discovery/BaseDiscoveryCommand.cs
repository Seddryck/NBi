using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Analysis.Discovery.FactoryValidations;

namespace NBi.Core.Analysis.Discovery
{
    public class BaseDiscoveryCommand
    {
        public string ConnectionString { get; set; }
        protected IDictionary<DiscoveryTarget, IFilter> Filters { get; set; }
        internal IList<Validation> Validations { get; set; }

        public BaseDiscoveryCommand()
        {
            Filters = new Dictionary<DiscoveryTarget, IFilter>();
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

    }
}
