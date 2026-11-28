using System;
using System.Linq;
using NBi.Core;
using NBi.Model.Xml.Constraints;
using NBi.Model.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionFasterThanChooser : ITestCaseBuilderChooser
    {
        public TestCaseFactory.BuilderRegistration Target { get; set; }
        
        public void Choose(AbstractSystemUnderTestXml sut, AbstractConstraintXml ctr)
        {
            if (sut.BaseItem is IExecutableArgs)
                Target.Builder = new ExecutionNonQueryFasterThanBuilder();
            else
                Target.Builder = new ExecutionFasterThanBuilder();
        }
    }
}
