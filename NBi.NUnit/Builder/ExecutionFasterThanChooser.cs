using System;
using System.Linq;
using NBi.Core;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;

namespace NBi.NUnit.Builder
{
    class ExecutionFasterThanChooser : ITestCaseBuilderChooser
    {
        public TestCaseFactory.BuilderRegistration Target { get; set; }
        
        public void Choose(AbstractSystemUnderTestXml sut, AbstractConstraintXml ctr)
        {
            if (sut.BaseItem is IExecutable)
                Target.Builder = new ExecutionFasterThanBuilder();
            else
                Target.Builder = new ExecutionNonQueryFasterThanBuilder();
        }
    }
}
