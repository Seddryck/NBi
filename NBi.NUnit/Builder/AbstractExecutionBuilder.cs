using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.NUnit.Builder.Helper;
using NBi.Core.Query.Resolver;
using NBi.Core.Query.Command;

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

        protected virtual IQuery InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            var query = GetQuery(executionXml);
            return query;
        }

        protected virtual IQuery GetQuery(ExecutionXml executionXml)
        {
            var commandFactory = new CommandFactory();

            var connectionString = executionXml.Item.GetConnectionString();
            var commandText = (executionXml.Item as QueryableXml).GetQuery();

            IEnumerable<IQueryParameter> parameters = null;
            IEnumerable<IQueryTemplateVariable> variables = null;
            var commandType = CommandType.Text;
            int timeout = 0;

            if (executionXml.BaseItem is QueryXml)
            {
                var builder = new QueryResolverArgsBuilder(ServiceLocator);
                parameters = builder.BuildParameters(((QueryXml)executionXml.BaseItem).GetParameters());
                variables = ((QueryXml)executionXml.BaseItem).GetVariables();
                timeout = ((QueryXml)executionXml.BaseItem).Timeout;
            }
            if (executionXml.BaseItem is ReportXml)
            {
                var builder = new QueryResolverArgsBuilder(ServiceLocator);
                parameters = builder.BuildParameters(((ReportXml)executionXml.BaseItem).GetParameters());
            }

            if (executionXml.BaseItem is ReportXml)
            {
                commandType = ((ReportXml)executionXml.BaseItem).GetCommandType();
            }

            var queryArgs = new QueryResolverArgs(commandText, connectionString, parameters, variables, new TimeSpan(0, 0, timeout), commandType);
            var factory = new QueryResolverFactory();
            var resolver = factory.Instantiate(queryArgs);
            var query = resolver.Execute();
            return query;

        }


    }
}
