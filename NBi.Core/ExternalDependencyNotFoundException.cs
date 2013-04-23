using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    public class ExternalDependencyNotFoundException : NBiException
    {
        public ExternalDependencyNotFoundException(string filename) 
            : base (string.Format("This test is in error because the following dependency has not been found '{0}'.", System.IO.Path.GetFullPath(filename)))
        {

        }


    }
}
