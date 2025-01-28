using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.Xml;
// BCL - Added class
namespace NBi.GenbiL.Action.Suite;

public class IncludeSuiteFromStringAction : Serializer, ISuiteAction
{
    public string Content { get; private set; }
    public string GroupPath { get; private set; }

    public IncludeSuiteFromStringAction(string content, string groupPath)
        => (Content, GroupPath) = (content, string.IsNullOrEmpty(groupPath) ? RootNode.Path : groupPath);
    
    public virtual void Execute(GenerationState state)
    {
        var testXml = Include(Content);
        GetParentNode(state.Suite).AddChild(new TestNode(testXml));
    }

    protected BranchNode GetParentNode(RootNode root) => root.GetChildBranch(GroupPath);

    protected internal TestStandaloneXml Include(string str)
    {
        var test = XmlDeserializeFromString<TestStandaloneXml>(str);
        test.Content = XmlSerializeFrom(test);
        return test;
    }

    public virtual string Display => $"Include test from string '{Content}'";
}
