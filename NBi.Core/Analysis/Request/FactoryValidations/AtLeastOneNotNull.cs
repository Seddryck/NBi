using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations
{
    internal class AtLeastOneNotNull : Validation
    {
        private readonly IList<string> elements;

        internal AtLeastOneNotNull(string firstElement, string secondElement)
            : base ()
        {
            elements = new List<string>();
            elements.Add(firstElement);
            elements.Add(secondElement);
        }

        internal override void Apply()
        {
            foreach (var element in elements)
            {
                if (!string.IsNullOrEmpty(element))
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
