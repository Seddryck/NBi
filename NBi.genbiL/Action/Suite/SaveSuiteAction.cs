using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Suite
{
    public class SaveSuiteAction : ISuiteAction
    {
        public string Filename { get; set; }

        public SaveSuiteAction(string filename)
        {
            Filename = filename;
        }
        
        public void Execute(GenerationState state)
        {
            var suiteXml = new TestSuiteXml()
            {
                Settings = state.Settings,
                Variables = state.Variables.ToList(),
            };
            foreach (var testNode in state.Suite.Children.Cast<TestNode>())
                suiteXml.Tests.Add(new TestXml(testNode.Content));

            var manager = new XmlManager();
            manager.Persist(Filename, suiteXml);
        }

        public string Display => $"Saving TestSuite to '{Filename}'";
    }
}
