using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionFasterThanBuilder: AbstractExecutionBuilder
    {
        protected FasterThanXml ConstraintXml {get; set;}

        public ExecutionFasterThanBuilder()
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
            if (fasterThanXml.CleanCache)
                ctr = ctr.CleanCache();
            if (fasterThanXml.TimeOutMilliSeconds > 0)
                ctr = ctr.TimeOutMilliSeconds(fasterThanXml.TimeOutMilliSeconds);
            return ctr;
        }
    }
}
