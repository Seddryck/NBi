using System;
using System.Linq;
using NBi.Core;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class QueryFasterThanBuilder: AbstractQueryBuilder
    {
        protected FasterThanXml ConstraintXml {get; set;}

        public QueryFasterThanBuilder()
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

        protected object InstantiateSystemUnderTest(QueryXml queryXml)
        {
            var conn = ConnectionFactory.Get(queryXml.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = queryXml.GetQuery();

            return cmd;
        }

    }
}
