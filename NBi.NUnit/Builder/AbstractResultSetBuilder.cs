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
using NBi.Core.ResultSet.Alteration;
using NBi.Core.Evaluate;
using NBi.Core.Calculation;
using NBi.NUnit.Builder.Helper;
using NBi.Core.Query.Resolver;
using NBi.Core.Query.Command;
using NBi.Core.Scalar.Caster;
using NBi.Core.Scalar.Conversion;
using NBi.Core.ResultSet.Conversion;
using NBi.Core.Transformation;
using NBi.Core.Configuration;
using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Extensibility.Query;

namespace NBi.NUnit.Builder
{
    abstract class AbstractResultSetBuilder : AbstractTestCaseBuilder
    {
        protected AbstractSystemUnderTestXml SystemUnderTestXml { get; set; }
        protected ResultSetSystemHelper Helper { get; private set; }

        public override void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, IConfiguration config, IDictionary<string, ITestVariable> variables, ServiceLocator serviceLocator)
        {
            base.Setup(sutXml, ctrXml, config, variables, serviceLocator);
            Helper = new ResultSetSystemHelper(ServiceLocator, Variables);
        }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is ExecutionXml || sutXml is ResultSetSystemXml))
                throw new ArgumentException("System-under-test must be a 'ExecutionXml' or 'ResultSetXml'");

            SystemUnderTestXml = sutXml;
        }

        protected override void BaseBuild()
        {
            if (SystemUnderTestXml is ExecutionXml)
                SystemUnderTest = InstantiateSystemUnderTest((ExecutionXml)SystemUnderTestXml);
            else
                SystemUnderTest = InstantiateSystemUnderTest((ResultSetSystemXml)SystemUnderTestXml);
        }

        protected virtual IResultSetService InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            var commandFactory = new CommandProvider();

            var argsBuilder = new QueryResolverArgsBuilder(ServiceLocator);

            var connectionString = executionXml.Item.GetConnectionString();
            var statement = (executionXml.Item as QueryableXml).GetQuery();

            IEnumerable<IQueryParameter> parameters = null;
            IEnumerable<IQueryTemplateVariable> templateVariables = null;
            int timeout = 0;
            var commandType = CommandType.Text;

            if (executionXml.BaseItem is QueryXml)
            {
                parameters = argsBuilder.BuildParameters(((QueryXml)executionXml.BaseItem).GetParameters());
                templateVariables = ((QueryXml)executionXml.BaseItem).GetTemplateVariables();
                timeout = ((QueryXml)executionXml.BaseItem).Timeout;
            }
            if (executionXml.BaseItem is ReportXml)
            {
                parameters = argsBuilder.BuildParameters(((ReportXml)executionXml.BaseItem).GetParameters());
            }

            if (executionXml.BaseItem is ReportXml)
            {
                commandType = ((ReportXml)executionXml.BaseItem).GetCommandType();
            }

            var queryArgs = new QueryResolverArgs(statement, connectionString, parameters, templateVariables, new TimeSpan(0, 0, timeout), commandType);
            var args = new QueryResultSetResolverArgs(queryArgs);
            var factory = ServiceLocator.GetResultSetResolverFactory();
            var resolver = factory.Instantiate(args);

            var builder = new ResultSetServiceBuilder();
            builder.Setup(resolver);
            var service = builder.GetService();

            return service;
        }

        protected virtual object InstantiateSystemUnderTest(ResultSetSystemXml resultSetXml)
        {
            var builder = new ResultSetServiceBuilder();
            builder.Setup(Helper.InstantiateResolver(resultSetXml));
            builder.Setup(Helper.InstantiateAlterations(resultSetXml));
            return builder.GetService();
        }

        

    }
}
