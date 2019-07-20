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

            AppendNodes(suiteXml.Tests, suiteXml.Groups, state.Suite.Children);

            var manager = new XmlManager();
            manager.Persist(Filename, suiteXml);
        }

        private void AppendNodes(IList<TestXml> tests, IList<GroupXml> groups, IEnumerable<TreeNode> nodes)
        {
            foreach (var node in nodes.Where(x => x is GroupNode))
            {
                var newGroup = new GroupXml() { Name = node.Name };
                groups.Add(newGroup);
                AppendNodes(newGroup.Tests, newGroup.Groups, (node as GroupNode).Children);
            }

            foreach (var node in nodes.Where(x => x is TestNode))
                tests.Add(new TestXml((node as TestNode).Content));
        }


        public string Display => $"Saving TestSuite to '{Filename}'";
    }
}
