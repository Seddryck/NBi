using System.Diagnostics;
using System.IO;

namespace NBi.UI.NUnit
{
    public class NUnitLauncher
    {
        public string TestSuite { get; set; }

        public void CleanConfiguration()
        {
            if (File.Exists("NBi.NUnit.Runtime.config"))
                File.Delete("NBi.NUnit.Runtime.config");
        }
        
        public void Configure(string fullpath)
        {
            CleanConfiguration();
            using (var sw = new StreamWriter("NBi.NUnit.Runtime.config"))
            {
                sw.Write(fullpath);
            }
        }
        
        public void Run()
        {
            Process.Start(@"NUnit.Runners\nunit.exe", "NBi.NUnit.Runtime.dll /run");
        }
    }
}
