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

        protected virtual string GetFileName() => AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        protected virtual System.Configuration.Configuration Open() => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        
    }
}
