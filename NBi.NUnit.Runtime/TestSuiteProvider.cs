using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core;
using NBi.NUnit.Runtime.Configuration;

namespace NBi.NUnit.Runtime
{
    public class TestSuiteProvider
    {
        public virtual string GetFilename(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, "The config file is not redirecting to a test-suite.");
                return GetUnspecifiedFilename();
            }
            else
            {
                if (Path.IsPathRooted(path))
                    return path;
                else
                    return AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path;
            }
        }

        private string GetUnspecifiedFilename()
        {
            // If no config file is registered then search the first "nbits" (NBi Test Suite) file
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var directory = Path.GetDirectoryName(path);
            var files = Directory.GetFiles(directory, "*.nbits");
            if (files.Count() > 0)
            {
                if (files.Count() == 1)
                    Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"A unique file test-suite file named '{ files[0]}' was found in the directory '{directory}'. Using it!");
                else
                    Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"{files.Count()} '.nbits' files have been found in the directory '{directory}'. Using the first file found named '{files[0]}'!");
                return files[0];
            }
            throw new ArgumentNullException($"No '.nbits' file has been found in the directory '{directory}'.");
        }
    }
}
