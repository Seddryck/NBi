using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;

namespace NBi.NUnit.Runtime
{
    public class ConfigurationFinder
    {
        protected internal virtual NBiSection Find()
        {
            string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            //Try to find a config file, if existing take the path inside for the TestSuite
            if (File.Exists(configFile))
            {
                //line bellow to avoid .Net framework bug: http://support.microsoft.com/kb/2580188/en-us
                var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                var section = (NBiSection)(configuration.GetSection("nbi"));
                if (section != null)
                    return section;
                    
            }
            return new NBiSection();
        }
    }
}
