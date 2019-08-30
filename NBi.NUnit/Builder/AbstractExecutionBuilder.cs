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
using NBi.Extensibility.Query;

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
            var builder = new QueryResolverArgsBuilder(ServiceLocator);
            builder.Setup(executionXml.Item, executionXml.Settings, Variables);
            builder.Build();

            var factory = ServiceLocator.GetQueryResolverFactory();
            var resolver = factory.Instantiate(builder.GetArgs());
            var query = resolver.Execute();
            return query;
        }


    }
}
