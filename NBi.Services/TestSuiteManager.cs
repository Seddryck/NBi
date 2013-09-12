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
            var sm = new SettingsManager();
            foreach (var s in settings)
                sm.Add(s.Name, s.Value);
            
            this.settings = sm.Settings;
        }

        public void DefineTests(IList<Test> tests)
        {
            this.tests = new List<TestXml>();
            this.tests.Clear();
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
