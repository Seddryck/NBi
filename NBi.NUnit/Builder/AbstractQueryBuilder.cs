using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    abstract class AbstractQueryBuilder : AbstractTestCaseBuilder
    {
        protected QueryXml SystemUnderTestXml { get; set; }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is QueryXml))
                throw new ArgumentException("Constraint must be a 'QueryXml'");

            SystemUnderTestXml = (QueryXml)sutXml;
        }

        protected override void BaseBuild()
        {
            SystemUnderTest = InstantiateSystemUnderTest(SystemUnderTestXml);
        }

        protected IDbCommand InstantiateSystemUnderTest(QueryXml queryXml)
        {
            var conn = ConnectionFactory.Get(queryXml.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = queryXml.GetQuery();

            return cmd;
        }


    }
}
