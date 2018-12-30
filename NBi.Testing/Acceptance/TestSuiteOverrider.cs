using NBi.Core.Configuration;
using NBi.Core.Variable;
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

        public TestSuiteOverrider(string filename, string configFilename) 
            : base(new TestSuiteProviderOverrider(filename)
                  , new ConfigurationProviderOverrider(configFilename)
                  , new ConnectionStringsFinderOverrider(configFilename))
        { }

        internal class TestSuiteProviderOverrider : TestSuiteProvider
        {
            private readonly string filename;
            public TestSuiteProviderOverrider(string filename)
            {
                this.filename = filename;
            }

            public override string GetFilename(string path)
            {
                if (string.IsNullOrEmpty(path))
                    return @"Acceptance\Resources\" + filename;
                else
                    return @"Acceptance\Resources\" + path;
            }
        }

        internal class ConfigurationProviderOverrider : ConfigurationProvider
        {
            private readonly string filename;
            public ConfigurationProviderOverrider(string filename) => this.filename = filename;

            public override NBiSection GetSection()
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
        public override void ExecuteTestCases(TestXml test, string testName, IDictionary<string, ITestVariable> localVariables)
        {
            base.ExecuteTestCases(test, testName, localVariables);
        }

        [Ignore]
        public void ExecuteTestCases(TestXml test, string testName, IConfiguration configuration)
        {
            base.Configuration = configuration;
            base.ExecuteTestCases(test, testName, null);
        }
    }
}
