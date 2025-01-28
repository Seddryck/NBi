using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.Extensibility;

public class ExternalDependencyNotFoundException : NBiException
{
    public ExternalDependencyNotFoundException(string filename) 
        : base ($"This test is in error because the following dependency has not been found '{Path.GetFullPath(filename)}'.")
    { }


}
