using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery.FactoryValidations
{
    internal class MeasureGroupWithoutDimension : Validation
    {
        protected readonly string measuregroup;
        protected readonly string dimension;

        internal MeasureGroupWithoutDimension(string mg,string dim)
        {
            measuregroup = mg;
            dimension = dim;
        }

        internal override void Apply()
        {
            if (!(string.IsNullOrEmpty(dimension) || string.IsNullOrEmpty(measuregroup)))
                GenerateException();
        }

        internal override void GenerateException()
        {
            throw new DiscoveryFactoryException("Dimension and Measure-Group cannot be specified at the same time");
        }
    }
}
