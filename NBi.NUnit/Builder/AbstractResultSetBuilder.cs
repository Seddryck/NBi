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
using NBi.Core.Scalar.Casting;
using NBi.Core.Scalar.Conversion;
using NBi.Core.ResultSet.Conversion;
using NBi.Core.Transformation;
using NBi.Core.Configuration;
using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Extensibility.Query;
using NBi.Xml.Settings;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.Builder
{
    abstract class AbstractResultSetBuilder : AbstractTestCaseBuilder
    {
        protected AbstractSystemUnderTestXml SystemUnderTestXml { get; set; }
        
        public override void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, IConfiguration config, IDictionary<string, IVariable> variables, ServiceLocator serviceLocator)
        {
            base.Setup(sutXml, ctrXml, config, variables, serviceLocator);
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

        protected virtual IResultSetResolver InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            var queryArgsBuilder = new QueryResolverArgsBuilder(ServiceLocator);
            queryArgsBuilder.Setup(executionXml.Item, executionXml.Settings, Variables);
            queryArgsBuilder.Build();

            var factory = ServiceLocator.GetResultSetResolverFactory();
            var resolver = factory.Instantiate(new QueryResultSetResolverArgs(queryArgsBuilder.GetArgs()));

            return resolver;
        }

        protected virtual object InstantiateSystemUnderTest(ResultSetSystemXml resultSetXml)
        {
            var helper = new ResultSetSystemHelper(ServiceLocator, SettingsXml.DefaultScope.SystemUnderTest, Variables);
            var resolver = helper.InstantiateResolver(resultSetXml);
            return resolver;
        }
    }
}
