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
        private Type ConstraintType { get; }
        private IScalarResolver<decimal> Resolver { get; }

        public DifferedConstraint(Type constraintType, IScalarResolver<decimal> resolver)
        {
            ConstraintType = constraintType;
            Resolver = resolver;
        }

        public NUnitCtr.Constraint Resolve()
        {
            var expected = Resolver.Execute();
            var ctr = Activator.CreateInstance(ConstraintType, expected);
            return (NUnitCtr.Constraint)ctr;
        }
        
    }
}
