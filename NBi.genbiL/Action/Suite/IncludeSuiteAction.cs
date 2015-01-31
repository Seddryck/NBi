using NBi.GenbiL.Stateful;
using NBi.Service;
using NBi.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Suite
{
    public class IncludeSuiteAction : ISuiteAction
    {
        public string Filename { get; protected set; }
        public TextReader StreamReader { get; protected set; }

        public IncludeSuiteAction(string filename)
        {
            Filename = filename;
        }
        public IncludeSuiteAction(TextReader streamReader)
        {
            StreamReader = streamReader;
        }

        public void Execute(GenerationState state)
        {
            if (StreamReader == null)
            {
                var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read);
                StreamReader = new StreamReader(stream, Encoding.UTF8, true);
            }

            var test = Include(StreamReader);
            state.Suite.Tests.Add(test);
        }

        protected internal TestXml Include(TextReader reader)
        {
            var str = reader.ReadToEnd();
            var serializer = new Serializer();
            var test = serializer.XmlDeserializeFromString<TestStandaloneXml>(str);
            return test;
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
