using System;
using System.Linq;
using NBi.Core;
using NBi.Extensibility;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    abstract class AbstractExecutionNonQueryBuilder : AbstractTestCaseBuilder
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

        protected IExecutable InstantiateSystemUnderTest(ExecutionXml executionXml)
        {
            var factory = new ExecutionFactory();
            var instance = factory.Instantiate(executionXml.BaseItem as IExecutableArgs);
            return instance;
        }


    }
}
