using System;
using System.Linq;
using NBi.NUnit.Execution;
using NBi.Model.Xml.Constraints;
using NBi.Model.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionNonQuerySuccessfulBuilder : AbstractExecutionNonQueryBuilder
    {
        protected SuccessfulXml ConstraintXml { get; set; }

        public ExecutionNonQuerySuccessfulBuilder()
        {

        }


        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SuccessfulXml))
                throw new ArgumentException("Constraint must be a 'SuccessfulXml'");

            ConstraintXml = ctrXml as SuccessfulXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected NBiConstraint InstantiateConstraint(SuccessfulXml successfulXml)
        {
            var ctr = new SuccessfulConstraint();
            return ctr;
        }
    }
}
