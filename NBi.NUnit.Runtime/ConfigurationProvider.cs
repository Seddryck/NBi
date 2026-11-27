using NBi.NUnit.Runtime.Configuration;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;

namespace NBi.NUnit.Runtime
{
    public class ConfigurationProvider
    {
        internal static AsyncLocal<string?> ExplicitFile { get; } = new();

        public virtual NBiSection GetSection()
        {
            string configFile = GetFileName();
            //Try to find a config file, if existing take the path inside for the TestSuite
            if (File.Exists(configFile))
            {
                //line bellow to avoid .Net framework bug: http://support.microsoft.com/kb/2580188/en-us
                var configuration = Open();

                var section = (NBiSection)(configuration.GetSection("nbi"));
                if (section != null)
                    return section;
            }
            return new NBiSection();
        }

        protected virtual string GetFileName() => GetConfigurationFile();
        protected virtual System.Configuration.Configuration Open() => OpenConfiguration(GetFileName());

        internal static string GetConfigurationFile()
        {
            if (ExplicitFile.Value is { } explicitFile)
                return explicitFile;

            var runtimeFile = typeof(TestSuite).Assembly.Location + ".config";
            return File.Exists(runtimeFile)
                ? runtimeFile
                : ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
        }

        internal static System.Configuration.Configuration OpenConfiguration(string path)
            => ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap { ExeConfigFilename = path },
                ConfigurationUserLevel.None);
    }
}
