using System;
using System.Configuration;
using System.Linq;

namespace NBi.NUnit.Runtime
{
    public class NBiSection : ConfigurationSection
    {
        // Create a "remoteOnly" attribute.
        [ConfigurationProperty("testSuite", IsRequired = true)]
        public string TestSuiteFilename
        {
            get
            {
                return (string)this["testSuite"];
            }
            set
            {
                this["testSuite"] = value;
            }
        }

        // Create a "remoteOnly" attribute.
        [ConfigurationProperty("settings", IsRequired = false)]
        public string SettingsFilename
        {
            get
            {
                return (string)this["settings"];
            }
            set
            {
                this["settings"] = value;
            }
        }

        // Create a "remoteOnly" attribute.
        [ConfigurationProperty("enableAutoCategories", IsRequired = false, DefaultValue=true)]
        public bool EnableAutoCategories
        {
            get
            {
                return (bool)this["enableAutoCategories"];
            }
            set
            {
                this["enableAutoCategories"] = value;
            }
        }

        // Create a "remoteOnly" attribute.
        [ConfigurationProperty("allowDtdProcessing", IsRequired = false, DefaultValue = false)]
        public bool AllowDtdProcessing
        {
            get
            {
                return (bool)this["allowDtdProcessing"];
            }
            set
            {
                this["allowDtdProcessing"] = value;
            }
        }
    }
}
