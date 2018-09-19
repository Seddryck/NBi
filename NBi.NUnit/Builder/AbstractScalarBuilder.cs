using System;
using System.Collections.Generic;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.NUnit.Builder.Helper;
using NBi.Core.Configuration;
using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Core.Scalar.Resolver;

namespace NBi.NUnit.Builder
{
    abstract class AbstractScalarBuilder : AbstractTestCaseBuilder
    {
        protected AbstractSystemUnderTestXml SystemUnderTestXml { get; set; }
        protected ScalarHelper Helper { get; private set; }

        public override void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, IConfiguration config, IDictionary<string, ITestVariable> variables, ServiceLocator serviceLocator)
        {
            base.Setup(sutXml, ctrXml, config, variables, serviceLocator);
            Helper = new ScalarHelper(ServiceLocator, Variables);
        }

        protected override void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(sutXml is ScalarXml))
                throw new ArgumentException("System-under-test must be a 'ScalarXml'");

            SystemUnderTestXml = sutXml;
        }

        protected override void BaseBuild()
        {
            if (SystemUnderTestXml is ScalarXml)
                SystemUnderTest = InstantiateSystemUnderTest((ScalarXml)SystemUnderTestXml);
        }

        protected virtual IScalarResolver<decimal> InstantiateSystemUnderTest(ScalarXml scalarXml) => Helper.InstantiateResolver(scalarXml);

    }
}
