using System;
using System.Linq;
using NBi.NUnit.Execution;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionNonQueryFasterThanBuilder : AbstractExecutionNonQueryBuilder
    {
        protected FasterThanXml ConstraintXml {get; set;}

        public ExecutionNonQueryFasterThanBuilder()
        {

        }


        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is FasterThanXml))
                throw new ArgumentException("Constraint must be a 'FasterThanXml'");

            ConstraintXml = (FasterThanXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected NBiConstraint InstantiateConstraint(FasterThanXml fasterThanXml)
        {
            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(fasterThanXml.MaxTimeMilliSeconds);

            return ctr;
        }
    }
}
