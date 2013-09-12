using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core;

namespace NBi.NUnit.Runtime
{
    public class TestSuiteFinder
    {
        protected internal virtual string Find()
        {
            string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            //Try to find a config file, if existing take the path inside for the TestSuite
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Looking for the config file located at '{0}'.", configFile));
            if (File.Exists(configFile))
            {
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "The config file has been found.");
                
                //line bellow to avoid .Net framework bug: http://support.microsoft.com/kb/2580188/en-us
                var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                var section = (NBiSection)(configuration.GetSection("nbi"));
                if (section != null && !string.IsNullOrEmpty(section.TestSuiteFilename))
                {
                    var configFullPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + section.TestSuiteFilename;
                    return configFullPath;
                }
                else
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "The config file is not redirecting to a test suite.");
            }
            else
                Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, "No config file has been found!");

            // If no config file is registered then search the first "nbits" (NBi Test Suite) file
            string assem = Path.GetFullPath((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath).Replace("%20", " "); 
            string directory = Path.GetDirectoryName(assem);
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Looking for a 'nbits' files in directory '{0}'.", directory));
            var files = System.IO.Directory.GetFiles(directory, "*.nbits");
            if (files.Count() > 0)
            {
                if (files.Count() == 1)
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("'{0}' found, using it!", files[0]));
                else
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("{0} 'nbits' files found, using the first found: '{1}'!", files.Count(), files[0]));
                return files[0];
            }

            throw new ArgumentNullException("No config file or nbits file has been found.");
        }
    }
}
