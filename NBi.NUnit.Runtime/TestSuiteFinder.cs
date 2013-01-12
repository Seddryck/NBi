using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NBi.NUnit.Runtime
{
    public class TestSuiteFinder
    {
        protected internal virtual string Find()
        {
            string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            //Try to find a config file, if existing take the path inside for the TestSuite
            Console.Out.WriteLine("Looking after config file located at '{0}'", configFile);
            if (File.Exists(configFile))
            {
                Console.Out.WriteLine("Config file found!");
                
                //line bellow to avoid .Net framework bug: http://support.microsoft.com/kb/2580188/en-us
                var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                var section = (NBiSection)(configuration.GetSection("nbi"));
                if (section != null && !string.IsNullOrEmpty(section.TestSuiteFilename))
                {
                    var configFullPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + section.TestSuiteFilename;
                    return configFullPath;
                }
                else
                    Console.Out.WriteLine("Config file not redirecting to a test suite!");
            }
            else
                Console.Out.WriteLine("No config file found!");

            // If no config file is registered then search the first "nbits" (NBi Test Suite) file
            Console.Out.WriteLine("Looking after 'nbits' files ...");
            string assem = Path.GetFullPath((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath).Replace("%20", " ");
            var files = System.IO.Directory.GetFiles(Path.GetDirectoryName(assem), "*.nbits");
            if (files.Count() > 0)
            {
                if (files.Count() == 1)
                    Console.Out.WriteLine("'{0}' found, using it!", files[0]);
                else
                    Console.Out.WriteLine("{0} 'nbits' files found, using the first found: '{1}'!", files.Count(), files[0]);
                return files[0];
            }

            throw new ArgumentNullException("No config or nbits file has been found.");
        }
    }
}
