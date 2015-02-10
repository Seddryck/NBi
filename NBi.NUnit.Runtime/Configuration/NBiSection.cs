using System;
using System.Configuration;
using System.Linq;

namespace NBi.NUnit.Runtime.Configuration
{
    public class NBiSection : ConfigurationSection
    {
        // Create a "testSuite" attribute.
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

        // Create a "settings" attribute.
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

        // Create a "enableAutoCategories" attribute.
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

        // Create a "allowDtdProcessing" attribute.
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

        // Create a "enableGroupAsCategory" attribute.
        [ConfigurationProperty("enableGroupAsCategory", IsRequired = false, DefaultValue = true)]
        public bool EnableGroupAsCategory
        {
            get
            {
                return (bool)this["enableGroupAsCategory"];
            }
            set
            {
                this["enableGroupAsCategory"] = value;
            }
        }

        [ConfigurationProperty("failureReportProfile", IsRequired = false)]
        public FailureReportProfileElement FailureReportProfile
        {
            get
            {
                return (FailureReportProfileElement)this["failureReportProfile"];
            }
            set
            {
                this["failureReportProfile"] = value;
            }
        }
    }
}
