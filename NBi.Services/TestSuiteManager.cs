using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Xml;
using NBi.Xml.Settings;

namespace NBi.Service
{
    public class TestSuiteManager
    {
        private SettingsXml settings;
        private IList<TestXml> tests;

        public void DefineSettings(SettingsManager settingsManager)
        {
            this.settings = settingsManager.GetSettings();
        }

        public void DefineTests(TestManager testManager)
        {
            this.tests = testManager.Tests;
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
    }
}
