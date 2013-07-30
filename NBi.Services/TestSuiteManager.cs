using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Service.Dto;
using NBi.Xml;
using NBi.Xml.Settings;

namespace NBi.Service
{
    public class TestSuiteManager
    {
        private SettingsXml settings;
        private IList<TestXml> tests;

        public void DefineSettings(IEnumerable<Setting> settings)
        {
            this.settings = new SettingsXml();
            foreach (var s in settings)
            {
                if (s.Name.StartsWith("Default - System-under-test"))
                    this.settings.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.SystemUnderTest, ConnectionString = s.Value });
                else if (s.Name.StartsWith("Default - Assert"))
                    this.settings.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = s.Value });
                else
                    this.settings.References.Add(new ReferenceXml() { Name = s.Name.Split(' ')[2], ConnectionString = s.Value });
            }
                

        }

        public void DefineTests(IEnumerable<Test> tests)
        {
            this.tests = new List<TestXml>();
            foreach (var t in tests)
                this.tests.Add(t.Reference);
        }

        public void SaveAs(string filename)
        {
            var testSuite = new TestSuiteXml();
            var array = tests.ToArray();
            testSuite.Load(array);

            testSuite.Settings = settings;

            var manager = new XmlManager();
            manager.Persist(filename, testSuite);
        }

        public void Open(string fullPath, SettingsManager settingsManager, TestListManager testManager)
        {
            var manager = new XmlManager();
            manager.Load(fullPath);
            testManager.Tests =  manager.TestSuite.Tests;
            settingsManager.Settings = manager.TestSuite.Settings;
        }
    }
}
