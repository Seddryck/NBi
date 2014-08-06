using System;
using System.Linq;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class TestCase
    {
        public Constraint Constraint { get; set; }
        public Object SystemUnderTest { get; set; }

        public TestCase(object sut, Constraint ctr)
        {
            SystemUnderTest = sut;
            Constraint = ctr;
        }
    }
}
