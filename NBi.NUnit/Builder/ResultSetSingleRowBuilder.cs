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
    class ResultSetSingleRowBuilder : ResultSetNoRowsBuilder
    {
        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SingleRowXml))
                throw new ArgumentException("Constraint must be a 'SingleRowXml'");

            ConstraintXml = (SingleRowXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected override NBiConstraint InstantiateConstraint()
        {
            var filter = InstantiateFilter();
            var ctr = new SingleRowConstraint(filter);
            return ctr;
        }

    }
}
