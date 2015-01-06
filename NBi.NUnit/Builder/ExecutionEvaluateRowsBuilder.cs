using System;
using System.Diagnostics;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Comparer;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionEvaluateRowsBuilder : AbstractExecutionBuilder
    {
        protected EvaluateRowsXml ConstraintXml {get; set;}

        public ExecutionEvaluateRowsBuilder()
        {

        }

        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is EvaluateRowsXml))
                throw new ArgumentException("Constraint must be a 'ValidateXml'");

            ConstraintXml = (EvaluateRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint()
        {
            EvaluateRowsConstraint ctr = null;
            ctr = new EvaluateRowsConstraint(ConstraintXml.Variables, ConstraintXml.Expressions);
            return ctr;
        }
    }
}
