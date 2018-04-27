using System;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework.Constraints;
using NBi.Framework;
using System.Collections.Generic;
using NBi.Core.Variable;
using NBi.Core.Injection;
using NBi.Core.Configuration;

namespace NBi.NUnit.Builder
{
    public interface ITestCaseBuilder
    {
        void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, IConfiguration config, IDictionary<string, ITestVariable> variables, ServiceLocator serviceLocator);
        void Build();
        object GetSystemUnderTest();
        NBiConstraint GetConstraint();
    }
}
