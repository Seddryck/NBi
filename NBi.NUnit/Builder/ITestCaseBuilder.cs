using System;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework.Constraints;
using NBi.Framework;
using System.Collections.Generic;
using NBi.Core.Variable;

namespace NBi.NUnit.Builder
{
    public interface ITestCaseBuilder
    {
        void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml);
        void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, ITestConfiguration config);
        void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, ITestConfiguration config, IDictionary<string, ITestVariable> variables);
        void Build();
        object GetSystemUnderTest();
        NBiConstraint GetConstraint();
    }
}
