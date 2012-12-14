using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class QuerySyntacticallyCorrectBuilder: AbstractQueryBuilder
    {
        protected SyntacticallyCorrectXml ConstraintXml {get; set;}

        public QuerySyntacticallyCorrectBuilder()
        {

        }


        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is SyntacticallyCorrectXml))
                throw new ArgumentException("Constraint must be a 'SyntacticallyCorrectXml'");

            ConstraintXml = (SyntacticallyCorrectXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint(ConstraintXml);
        }

        protected global::NUnit.Framework.Constraints.Constraint InstantiateConstraint(SyntacticallyCorrectXml syntacticallyCorrectXml)
        {
            var ctr = new SyntacticallyCorrectConstraint();
            return ctr;
        }

        protected override IDbCommand InstantiateSystemUnderTest(QueryXml queryXml)
        {
            var conn = ConnectionFactory.Get(queryXml.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = queryXml.GetQuery();

            return cmd;
        }

    }
}
