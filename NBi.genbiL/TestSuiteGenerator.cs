using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NBi.Service;
using NBi.Xml;

namespace NBi.GenbiL
{
    public class TestSuiteGenerator
    {
        public XmlDocument Recipe { get; set; }
        //public TestSuiteXml Product { get; private set; }

        public TestSuiteGenerator()
        {
            
        }

        public void Load(string filename)
        {
            Recipe = new XmlDocument();
            Recipe.Load(filename);
        }

        public void Execute()
        {
            var product = new TestSuiteXml();

            foreach (XmlNode step in Recipe.GetElementsByTagName("testSuite")[0].ChildNodes)
            {
                
                switch (step.Name)
                {
                    case "save":
                        Save(step, product);
                        break;
                    case "step":
                        var state = new GenerationState();
                        foreach (XmlNode block in step.ChildNodes)
                        {
                            switch (block.Name)
                            {
                                case "testCases":
                                    foreach (XmlNode command in block.ChildNodes)
                                    {
                                        switch (command.Name)
                                        {
                                            case "open":
                                                OpenTestCases(command, state);
                                                break;
                                        }
                                    }
                                    break;
                                case "template":
                                    foreach (XmlNode command in block.ChildNodes)
                                    {
                                        switch (command.Name)
                                        {
                                            case "open":
                                                OpenTemplate(command, state);
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        var tests = Generate(state);
                        product.Tests.AddRange(tests);
                        break;
                }
            }
        }

        private void CheckCommandName(XmlNode command, string name)
        {
            if (command.Name != name)
                throw new ArgumentException(string.Format("An attribute named '{0}' is expected.", name), "command");
        }

        private string GetAttributeValue(XmlNode command, string attributeName)
        {
            var attribute = command.Attributes.GetNamedItem(attributeName);
            if (attribute == null)
                throw new ArgumentException(string.Format("The node has no attribute named '{0}'.", attributeName), "command");
            return attribute.Value;
        }

        private IList<TestXml> Generate(GenerationState state)
        {
            var mgr = new TestListManager();
            mgr.Build(state.Template, state.Variables.ToArray(), state.TestCases, false);

            return mgr.GetTestList();
        }

        private void OpenTemplate(XmlNode command, GenerationState state)
        {
            CheckCommandName(command, "open");
            var fileAttribute = command.Attributes.GetNamedItem("file");
            var embeddedAttribute = command.Attributes.GetNamedItem("embedded");
            if (fileAttribute == null && embeddedAttribute==null)
                throw new ArgumentException(string.Format("The node has no attribute named '{0}' or {1}.", "file", "embedded"), "command");

            if (fileAttribute != null)
                OpenExternalTemplate(GetAttributeValue(command, "file"), state);
            else
                OpenEmbeddedTemplate(GetAttributeValue(command, "Embedded"), state);
        }

        private void OpenEmbeddedTemplate(string name, GenerationState state)
        {
            var mgr = new TemplateManager();
            state.Template = mgr.GetEmbeddedTemplate(name);
        }

        private void OpenExternalTemplate(string filename, GenerationState state)
        {
            var mgr = new TemplateManager();
            state.Template = mgr.GetExternalTemplate(filename);
        }

        private void OpenTestCases(XmlNode command, GenerationState state)
        {
            CheckCommandName(command, "open");
            var filename = GetAttributeValue(command, "file");

            OpenTestCases(filename, state);
        }

        private void OpenTestCases(string filename, GenerationState state)
        {
            var mgr = new TestCasesManager();
            mgr.ReadFromCsv(filename);
            state.TestCases = mgr.Content;
            state.Variables = mgr.Variables;
        }
      
        protected void Save(XmlNode command, TestSuiteXml testSuite)
        {
            CheckCommandName(command, "save");
            var filename = GetAttributeValue(command, "file");

            Save(filename, testSuite);
        }

       
        protected void Save(string filename, TestSuiteXml testSuite)
        {
            var xmlManager = new XmlManager();
            xmlManager.Persist(filename, testSuite);
        }
    }
}
