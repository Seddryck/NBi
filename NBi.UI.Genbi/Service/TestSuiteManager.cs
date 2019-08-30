using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Variables;
using NBi.GenbiL.Stateful;
using NBi.IO.Genbi.Dto;

namespace NBi.UI.Genbi.Service
{
    public class TestSuiteManager
    {
        private SettingsXml settingsXml;
        private List<GlobalVariableXml> variablesXml;
        private IList<TestXml> listTestXml;

        public void DefineSettings(IEnumerable<Setting> settings)
        {
            this.settingsXml = new SettingsXml();
            foreach (var s in settings)
            {
                if (s.Name.StartsWith("Default - System-under-test"))
                    this.settingsXml.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.SystemUnderTest, ConnectionString = new ConnectionStringXml() { Inline = s.Value } });
                else if (s.Name.StartsWith("Default - Assert"))
                    this.settingsXml.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = new ConnectionStringXml() { Inline = s.Value } });
                else
                    this.settingsXml.References.Add(new ReferenceXml() { Name = s.Name.Split(' ')[2], ConnectionString = new ConnectionStringXml() { Inline = s.Value } });
            }
        }

        public void DefineSettings(SettingsXml settingsXml)
        {
            this.settingsXml = settingsXml;
        }

        public void DefineVariables(IDictionary<string, GlobalVariableXml> variables)
        {
            this.variablesXml = variables.Values.ToList();
        }

        public IEnumerable<Setting> GetSettings()
        {
            var settings = new List<Setting>()
            {
                new Setting { Name = "Default - System-under-test" },
                new Setting { Name = "Default - Assert" },
            };

            foreach (var s in settingsXml.Defaults)
            {
                if (s.ApplyTo == SettingsXml.DefaultScope.SystemUnderTest)
                    settings[0].Value = s.ConnectionString.Inline;
                else
                    settings[1].Value = s.ConnectionString.Inline;
            }
            foreach (var s in settingsXml.References)
            {
                settings.Add(new Setting { Name = "Reference - " + s.Name, Value = s.ConnectionString.Inline });
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
                tests.Add(new Test() { Title = t.Name, Content = t.Content, Reference = t });
            return tests;
        }

        public void SaveAs(string filename)
        {
            var testSuite = new TestSuiteXml();
            var array = listTestXml.ToArray();
            testSuite.Load(array);

            testSuite.Settings = settingsXml;
            testSuite.Variables = variablesXml;

            var manager = new XmlManager();
            manager.Persist(filename, testSuite);
        }

        public void Open(string fullPath)
        {
            var manager = new XmlManager();
            manager.Load(fullPath);
            listTestXml = manager.TestSuite.Tests;
            settingsXml = manager.TestSuite.Settings;
        }
    }
}
