using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    public class TestException: ArgumentException
    {
        public TestException(string message)
            : base(message)
        {

        }
    }
}
