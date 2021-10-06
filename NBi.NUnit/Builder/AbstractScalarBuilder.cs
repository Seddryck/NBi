using System;
using System.Collections.Generic;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.NUnit.Builder.Helper;
using NBi.Core.Configuration;
using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Extensibility.Resolving;
using NBi.Xml.Settings;

namespace NBi.NUnit.Builder
{
    abstract class AbstractScalarBuilder : AbstractTestCaseBuilder
    {
        protected ScalarXml SystemUnderTestXml { get; set; }

        public override void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, IConfiguration config, IDictionary<string, IVariable> variables, ServiceLocator serviceLocator)
            => base.Setup(sutXml, ctrXml, config, variables, serviceLocator);

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
            => SystemUnderTestXml = sutXml as ScalarXml 
            ?? throw new ArgumentException("System-under-test must be a 'ScalarXml'");

        protected override void BaseBuild()
            => SystemUnderTest = InstantiateSystemUnderTest(SystemUnderTestXml);

        protected virtual IScalarResolver<decimal> InstantiateSystemUnderTest(ScalarXml scalarXml)
            => new ScalarHelper(ServiceLocator, scalarXml.Settings, SettingsXml.DefaultScope.SystemUnderTest, new Context(Variables)).InstantiateResolver<decimal>(scalarXml);

    }
}
