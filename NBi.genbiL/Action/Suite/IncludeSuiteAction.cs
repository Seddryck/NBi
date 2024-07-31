using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NBi.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Suite
{
    public class IncludeSuiteAction : Serializer, ISuiteAction
    {
        public string Filename { get; private set; }
        public string GroupPath { get; private set; }

        public IncludeSuiteAction(string filename, string groupPath)
            => (Filename, GroupPath) = (filename, string.IsNullOrEmpty(groupPath) ? RootNode.Path : groupPath);
        
        public virtual void Execute(GenerationState state)
        {
            using var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read);
            var testXml = Include(stream);
            GetParentNode(state.Suite).AddChild(new TestNode(testXml));
        }

        protected BranchNode GetParentNode(RootNode root) => root.GetChildBranch(GroupPath);

        protected internal TestStandaloneXml Include(Stream stream)
        {
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8, true);
            var str = reader.ReadToEnd();
            var test = XmlDeserializeFromString<TestStandaloneXml>(str);
            test.Content = XmlSerializeFrom(test);
            return test;
        }

        public virtual string Display => $"Include test from '{Filename}'";
    }
}
