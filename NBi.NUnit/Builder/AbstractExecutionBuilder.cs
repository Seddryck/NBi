using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Loading;

namespace NBi.NUnit.Builder
{
    abstract class AbstractExecutionBuilder : AbstractTestCaseBuilder
    {
        protected AbstractSystemUnderTestXml SystemUnderTestXml { get; set; }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is ExecutionXml))
                throw new ArgumentException("System-under-test must be a 'ExecutionXml'");

            SystemUnderTestXml = (ExecutionXml)sutXml;
        }

        protected override void BaseBuild()
        {
            SystemUnderTest = InstantiateSystemUnderTest((ExecutionXml)SystemUnderTestXml);
        }

        protected virtual IDbCommand InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            var cmd = GetCommand(executionXml);
            return cmd;
        }

        protected virtual IDbCommand GetCommand(ExecutionXml executionXml)
        {
            var commandBuilder = new CommandBuilder();

            var connectionString = executionXml.Item.GetConnectionString();
            var commandText = (executionXml.Item as QueryableXml).GetQuery();

            IEnumerable<IQueryParameter> parameters = null;
            IEnumerable<IQueryTemplateVariable> variables = null;
            int timeout = 0;
            if (executionXml.BaseItem is QueryXml)
            {
                parameters = ((QueryXml)executionXml.BaseItem).GetParameters();
                variables = ((QueryXml)executionXml.BaseItem).GetVariables();
                timeout = ((QueryXml)executionXml.BaseItem).Timeout;
            }
            if (executionXml.BaseItem is ReportXml)
            {
                parameters = ((ReportXml)executionXml.BaseItem).GetParameters();
            }
            var cmd = commandBuilder.Build(connectionString, commandText, parameters, variables, timeout);

            if (executionXml.BaseItem is ReportXml)
            {
                cmd.CommandType = ((ReportXml)executionXml.BaseItem).GetCommandType();
            }

            return cmd;

        }


    }
}
