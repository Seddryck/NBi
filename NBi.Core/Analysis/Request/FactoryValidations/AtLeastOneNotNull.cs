using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations
{
    internal class AtLeastOneNotNull : ValidationFilter
    {
        private readonly IList<DiscoveryTarget> elements;

        internal AtLeastOneNotNull(IEnumerable<IFilter> filters, DiscoveryTarget firstElement, DiscoveryTarget secondElement)
            : base (filters)
        {
            elements = [firstElement, secondElement];
        }

        internal override void Apply()
        {
            foreach (var element in elements)
            {
                if (GetSpecificFilter(element)!=null)
                    return;
            }
            GenerateException();
        }

        internal override void GenerateException()
        {
            throw new DiscoveryRequestFactoryException("");
        }
    }
}
