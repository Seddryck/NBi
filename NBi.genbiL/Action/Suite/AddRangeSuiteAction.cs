using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Suite;

public class AddRangeSuiteAction : IncludeSuiteAction
{
    public AddRangeSuiteAction(string filename, string groupPath)
    : base(filename, groupPath) { }

    public override void Execute(GenerationState state)
    {
        using var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read);
        var testSuite = AddRange(stream);
        var parentNode = GetParentNode(state.Suite);
        foreach (var testXml in testSuite.GetAllTests())
            parentNode.AddChild(new TestNode(new StandaloneTest(testXml)));
    }

    protected internal TestSuite AddRange(Stream stream)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8, true);
        var str = reader.ReadToEnd();
        return XmlDeserializeFromString<TestSuite>(str);
    }

    public override string Display { get => $"Add a range of tests from '{Filename}'";}
}
