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
        public string Filename { get; set; }

        public IncludeSuiteAction(string filename)
        {
            Filename = filename;
        }
        
        public void Execute(GenerationState state)
        {
            using (var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read))
            {
                var testXml = Include(stream);
                state.Suite.AddChild(new TestNode(testXml));
            }
        }

        protected internal TestStandaloneXml Include(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var str = reader.ReadToEnd();
                var test = XmlDeserializeFromString<TestStandaloneXml>(str);
                test.Content = XmlSerializeFrom(test);
                return test;
            }
        }

        public string Display => $"Include test from '{Filename}'";
    }
}
