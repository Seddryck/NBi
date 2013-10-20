using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
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

        protected virtual IDbCommand InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            var commandBuilder = new CommandBuilder();

            var connectionString = executionXml.Item.GetConnectionString();
            var commandText = executionXml.Item.GetQuery();

            IEnumerable<QueryParameterXml> parameters=null;
            if (executionXml.BaseItem is QueryXml)
                parameters = ((QueryXml)executionXml.BaseItem).Parameters;

            var cmd = commandBuilder.Build(connectionString, commandText, parameters);

            return cmd;
        }


    }
}
