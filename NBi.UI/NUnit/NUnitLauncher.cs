using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace NBi.UI.NUnit
{
    public class NUnitLauncher
    {
        public string TestSuite { get; set; }

        public void Run()
        {
            Process.Start(@"NUnit.Runners\nunit.exe", "NBi.NUnit.Runtime.dll /run");
        }
    }
}
