using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;

namespace NBi.NUnit.Builder
{
    class ResultSetSomeRowsBuilder : ResultSetNoRowsBuilder
    {
        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SomeRowsXml))
                throw new ArgumentException("Constraint must be a 'SomeRowsXml'");

            ConstraintXml = (SomeRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected override NBiConstraint InstantiateConstraint()
        {
            var filter = InstantiateFilter();
            var ctr = new SomeRowsConstraint(filter);
            return ctr;
        }

    }
}
