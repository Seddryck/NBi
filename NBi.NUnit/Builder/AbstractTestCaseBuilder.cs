﻿using System;
using System.Linq;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.Builder
{
    abstract class AbstractTestCaseBuilder: ITestCaseBuilder
    {
        protected object SystemUnderTest {get; set;}
        protected Constraint Constraint  {get; set;}
        protected bool isSetup;

        public  void Setup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            BaseSetup(sutXml, ctrXml);
            SpecificSetup(sutXml, ctrXml);
            isSetup = true;
        }

        protected abstract void BaseSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml);
        protected abstract void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml);

        public void Build()
        {
            if (!isSetup)
                throw new InvalidOperationException("The method Build must be preceded by a call to method Setup");

            BaseBuild();
            SpecificBuild();
        }

        protected abstract void BaseBuild();
        protected abstract void SpecificBuild();

        public object GetSystemUnderTest()
        {
            return SystemUnderTest;
        }

        public Constraint GetConstraint()
        {
            return Constraint;
        }
    }
}
