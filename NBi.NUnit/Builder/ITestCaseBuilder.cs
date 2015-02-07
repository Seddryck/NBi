using System;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework.Constraints;
using NBi.Framework;

namespace NBi.NUnit.Builder
{
    public interface ITestCaseBuilder
    {
        void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml);
        void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml, ITestConfiguration config);
        void Build();
        object GetSystemUnderTest();
        NBiConstraint GetConstraint();
    }
}
