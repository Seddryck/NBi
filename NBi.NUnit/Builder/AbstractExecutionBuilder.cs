using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    abstract class AbstractExecutionBuilder : AbstractTestCaseBuilder
    {
        protected ExecutionXml SystemUnderTestXml { get; set; }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is ExecutionXml))
                throw new ArgumentException("System-under-test must be a 'ExecutionXml'");

            SystemUnderTestXml = (ExecutionXml)sutXml;
        }

        protected override void BaseBuild()
        {
            SystemUnderTest = InstantiateSystemUnderTest(SystemUnderTestXml);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected virtual IDbCommand InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            var conn = new ConnectionFactory().Get(executionXml.Item.GetConnectionString());
            var cmd = conn.CreateCommand();
            cmd.CommandText = executionXml.Item.GetQuery();

            return cmd;
        }


    }
}
