using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class DifferedConstraint
    {
        private Type constraintType;
        private readonly IScalarResolver<decimal> resolver;

        public DifferedConstraint(Type constraintType, IScalarResolver<decimal> resolver)
        {
            this.constraintType = constraintType;
            this.resolver = resolver;
        }

        public NUnitCtr.Constraint Resolve()
        {
            var expected = resolver.Execute();
            var ctr = Activator.CreateInstance(constraintType, expected);
            return (NUnitCtr.Constraint)ctr;
        }
        
    }
}
