using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
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

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(FasterThanXml fasterThanXml)
        {
            var ctr = new FasterThanConstraint();
            ctr = ctr.MaxTimeMilliSeconds(fasterThanXml.MaxTimeMilliSeconds);
            if (fasterThanXml.CleanCache)
                ctr = ctr.CleanCache();
            return ctr;
        }

        protected override IDbCommand InstantiateSystemUnderTest(ExecutionXml queryXml)
        {
            var conn = ConnectionFactory.Instance.Get(queryXml.Item.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = queryXml.Item.GetQuery();

            return cmd;
        }

    }
}
