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
        private SettingsXml settingsXml;
        private IList<TestXml> listTestXml;

        public void DefineSettings(IEnumerable<Setting> settings)
        {
            this.settingsXml = new SettingsXml();
            foreach (var s in settings)
            {
                if (s.Name.StartsWith("Default - System-under-test"))
                    this.settingsXml.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.SystemUnderTest, ConnectionString = s.Value });
                else if (s.Name.StartsWith("Default - Assert"))
                    this.settingsXml.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = s.Value });
                else
                    this.settingsXml.References.Add(new ReferenceXml() { Name = s.Name.Split(' ')[2], ConnectionString = s.Value });
            }
        }

        public IEnumerable<Setting> GetSettings()
        {
            var settings = new List<Setting>();
            settings.Add(new Setting { Name = "Default - System-under-test" });
            settings.Add(new Setting { Name = "Default - Assert" });
            foreach (var s in settingsXml.Defaults)
            { 
                if (s.ApplyTo == SettingsXml.DefaultScope.SystemUnderTest)
                    settings[0].Value = s.ConnectionString;
                else
                    settings[1].Value = s.ConnectionString;
            }
            foreach (var s in settingsXml.References)
            {
                settings.Add(new Setting { Name = "Reference - " + s.Name, Value = s.ConnectionString });
            }
            return settings;
        }
        
        public void DefineTests(IEnumerable<Test> tests)
        {
            this.listTestXml = new List<TestXml>();
            foreach (var t in tests)
                this.listTestXml.Add(t.Reference);
        }

        public IEnumerable<Test> GetTests()
        {
            var tests = new List<Test>();
            foreach (var t in listTestXml)
                tests.Add(new Test() {Title=t.Name, Content = t.Content, Reference=t });
            return tests;
        }

        public void SaveAs(string filename)
        {
            var testSuite = new TestSuiteXml();
            var array = listTestXml.ToArray();
            testSuite.Load(array);

            testSuite.Settings = settingsXml;

            var manager = new XmlManager();
            manager.Persist(filename, testSuite);
        }

        public void Open(string fullPath)
        {
            var manager = new XmlManager();
            manager.Load(fullPath);
            listTestXml =  manager.TestSuite.Tests;
            settingsXml = manager.TestSuite.Settings;
        }
    }
}
