using NBi.Core.Configuration;
using NBi.NUnit.Runtime;
using NBi.Xml;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysConfig = System.Configuration;

namespace NBi.Testing.Acceptance
{
    public class TestSuiteOverrider : TestSuite
    {
        public TestSuiteOverrider(string filename)
            : this(filename, null)
        {
        }

        public TestSuiteOverrider(string filename, string configFilename) : base()
        {
            TestSuiteFinder = new TestSuiteFinderOverrider(filename);
            ConfigurationFinder = new ConfigurationFinderOverrider(configFilename);
            ConnectionStringsFinder = new ConnectionStringsFinderOverrider(configFilename);
        }

        internal class TestSuiteFinderOverrider : TestSuiteFinder
        {
            private readonly string filename;
            public TestSuiteFinderOverrider(string filename)
            {
                this.filename = filename;
            }

            protected internal override string Find()
            {
                return @"Acceptance\Resources\" + filename;
            }
        }

        internal class ConfigurationFinderOverrider : ConfigurationFinder
        {
            private readonly string filename;
            public ConfigurationFinderOverrider(string filename)
            {
                this.filename = filename;
            }
            protected internal override NBiSection Find()
            {
                if (!string.IsNullOrEmpty(filename))
                {
                    var configuration = SysConfig.ConfigurationManager.OpenExeConfiguration(@"Acceptance\Resources\" + filename);

                    var section = (NBiSection)(configuration.GetSection("nbi"));
                    if (section != null)
                        return section;
                }
                return new NBiSection();
            }
        }

        internal class ConnectionStringsFinderOverrider : ConnectionStringsFinder
        {
            private readonly string filename;
            public ConnectionStringsFinderOverrider(string filename)
            {
                this.filename = filename;
            }
            protected override string GetConfigFile() => $@"Acceptance\Resources\{filename}.config";

            protected override SysConfig.Configuration GetConfiguration()
            {
                var configMap = new SysConfig.ExeConfigurationFileMap()
                {
                    ExeConfigFilename = GetConfigFile()
                };
                var config = SysConfig.ConfigurationManager.OpenMappedExeConfiguration(configMap, SysConfig.ConfigurationUserLevel.None);
                return config;
            }
        }

        [Ignore]
        public override void ExecuteTestCases(TestXml test)
        {
            base.ExecuteTestCases(test);
        }

        [Ignore]
        public void ExecuteTestCases(TestXml test, IConfiguration configuration)
        {
            base.Configuration = configuration;
            base.ExecuteTestCases(test);
        }
    }
}
