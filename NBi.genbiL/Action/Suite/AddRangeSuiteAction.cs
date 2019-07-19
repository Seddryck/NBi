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
    public class AddRangeSuiteAction : Serializer, ISuiteAction
    {
        public string Filename { get; set; }

        public AddRangeSuiteAction(string filename)
        {
            Filename = filename;
        }
        
        public void Execute(GenerationState state)
        {
            using (var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read))
            {
                var testSuite = AddRange(stream);
                foreach (var testXml in testSuite.GetAllTests())
                    state.Suite.AddChild(new TestNode(new TestStandaloneXml(testXml)));
            }
        }

        protected internal TestSuiteXml AddRange(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var str = reader.ReadToEnd();
                return XmlDeserializeFromString<TestSuiteXml>(str);
            }
        }

        public string Display
        {
            get
            {
                return string.Format("Include test from '{0}'"
                    , Filename
                    );
            }
        }
    }
}
