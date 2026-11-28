using System;
using System.Linq;
using NBi.Model.Xml.Constraints;
using NBi.Model.Xml.Systems;

namespace NBi.NUnit.Builder
{
    interface ITestCaseBuilderChooser
    {
        TestCaseFactory.BuilderRegistration Target { get; set; }
        void Choose(AbstractSystemUnderTestXml sut, AbstractConstraintXml ctr);
    }
}
