using NBi.NUnit.Runtime.Configuration;
using System;
using System.Configuration;
using System.Linq;

namespace NBi.NUnit.Runtime
{
    /// I know that the namespace is not correct, it's just to ensure compatibility with the previous release.
    /// <summary>
    /// Handle configuration information
    /// </summary>
    public class NBiSection : ConfigurationSection
    {
        // Create a "testSuite" attribute.
        [ConfigurationProperty("testSuite", IsRequired = true)]
        public string TestSuiteFilename
        {
            get => (string)this["testSuite"];
            set => this["testSuite"] = value;
        }

        // Create a "settings" attribute.
        [ConfigurationProperty("settings", IsRequired = false)]
        public string SettingsFilename
        {
            get => (string)this["settings"];
            set => this["settings"] = value;
        }

        // Create a "enableAutoCategories" attribute.
        [ConfigurationProperty("enableAutoCategories", IsRequired = false, DefaultValue = true)]
        public bool EnableAutoCategories
        {
            get => (bool)this["enableAutoCategories"];
            set => this["enableAutoCategories"] = value;
        }

        // Create a "allowDtdProcessing" attribute.
        [ConfigurationProperty("allowDtdProcessing", IsRequired = false, DefaultValue = false)]
        public bool AllowDtdProcessing
        {
            get => (bool)this["allowDtdProcessing"];
            set => this["allowDtdProcessing"] = value;
        }

        // Create a "enableGroupAsCategory" attribute.
        [ConfigurationProperty("enableGroupAsCategory", IsRequired = false, DefaultValue = true)]
        public bool EnableGroupAsCategory
        {
            get => (bool)this["enableGroupAsCategory"];
            set => this["enableGroupAsCategory"] = value;
        }

        [ConfigurationProperty("failure-report-profile", IsRequired = false)]
        public FailureReportProfileElement FailureReportProfile
        {
            get => (FailureReportProfileElement)this["failure-report-profile"];
            set => this["failure-report-profile"] = value;
        }

        [ConfigurationProperty("providers", IsRequired = false)]
        public ProviderCollection Providers
        {
            get => (ProviderCollection)this["providers"];
            set => this["providers"] = value;
        }

        [ConfigurationProperty("extensions", IsRequired = false)]
        public ExtensionCollection Extensions
        {
            get => (ExtensionCollection)this["extensions"];
            set => this["extensions"] = value;
        }
    }
}
