using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Comparer;
using NBi.NUnit.Query;
using NBi.Model.Xml.Constraints;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Systems;
using NBi.NUnit.Execution;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Model.Xml.Constraints.Comparer;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;

namespace NBi.NUnit.Builder
{
    class ResultSetAllRowsBuilder : ResultSetNoRowsBuilder
    {
        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is AllRowsXml))
                throw new ArgumentException("Constraint must be a 'AllRowsXml'");

            ConstraintXml = (AllRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected override NBiConstraint InstantiateConstraint()
        {
            var filter = InstantiateFilter();
            var ctr = new AllRowsConstraint(filter);
            return ctr;
        }
    }
}
