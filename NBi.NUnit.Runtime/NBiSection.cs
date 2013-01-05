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
    }
}
