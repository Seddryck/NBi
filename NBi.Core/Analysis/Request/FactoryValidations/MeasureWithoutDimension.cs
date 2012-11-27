using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request.FactoryValidations
{
    internal class MeasureWithoutDimension : Validation
    {
        protected readonly string measure;
        protected readonly string dimension;

        internal MeasureWithoutDimension(string m, string dim)
        {
            measure = m;
            dimension = dim;
        }

        internal override void Apply()
        {
            if (!(string.IsNullOrEmpty(dimension) || string.IsNullOrEmpty(measure)))
                GenerateException();
        }

        internal override void GenerateException()
        {
            throw new DiscoveryRequestFactoryException("Dimension and Measure cannot be specified at the same time");
        }
    }
}
