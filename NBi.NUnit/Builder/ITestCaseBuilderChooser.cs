using System;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    interface ITestCaseBuilderChooser
    {
        TestCaseFactory.BuilderRegistration Target { get; set; }
        void Choose(AbstractSystemUnderTestXml sut, AbstractConstraintXml ctr);
    }
}
