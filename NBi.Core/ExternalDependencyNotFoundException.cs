using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    public class ExternalDependencyNotFoundException : TestException
    {
        public ExternalDependencyNotFoundException(string filename) 
            : base (string.Format("This test has failed because the following dependency has not been found '{0}'.", filename))
        {

        }


    }
}
