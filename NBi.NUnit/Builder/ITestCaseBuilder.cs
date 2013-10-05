using System;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.Builder
{
    public interface ITestCaseBuilder
    {
        void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml);
        void Build();
        object GetSystemUnderTest();
        Constraint GetConstraint();
    }
}
