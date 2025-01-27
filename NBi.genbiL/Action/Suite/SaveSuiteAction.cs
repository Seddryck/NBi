using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.Xml;
using NBi.Xml.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Suite;

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
            Variables = [.. state.Variables],
        };

        AppendNodes(suiteXml.Groups, suiteXml.Tests, state.Suite.Children);

        var manager = new XmlManager();
        manager.Persist(Filename, suiteXml);
    }

    protected virtual void AppendNodes(IList<GroupXml> groups, IList<TestXml> tests, IEnumerable<TreeNode> nodes)
    {
        foreach (var node in nodes.Where(x => x is GroupNode).Cast<GroupNode>())
        {
            var newGroup = new GroupXml() { Name = node.Name };
            groups.Add(newGroup);

            var setupNode = (node).Children.FirstOrDefault(x => x is SetupNode);
            if (setupNode != null)
                newGroup.Setup = new SetupXml(((SetupNode)setupNode).Content);

            var cleanupNode = (node).Children.FirstOrDefault(x => x is CleanupNode);
            if (cleanupNode != null)
                newGroup.Cleanup = new CleanupXml(((CleanupNode)cleanupNode).Content);

            AppendNodes(newGroup.Groups, newGroup.Tests, node.Children);
        }

        foreach (var node in nodes.Where(x => x is TestNode).Cast<TestNode>())
            tests.Add(new TestXml(node.Content));
    }


    public string Display => $"Saving TestSuite to '{Filename}'";
}
