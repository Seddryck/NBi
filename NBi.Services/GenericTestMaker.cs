using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NBi.Xml;

namespace NBi.Service
{
    public class GenericTestMaker
    {
        public string TemplateXml { get; private set; }

        public GenericTestMaker(string templateXml)
        {
            TemplateXml = templateXml;
        }

        public IEnumerable<TestXml> Build(IEnumerable<IList<string>> rows)
        {
            var tests = new List<TestXml>();

            foreach (var row in rows)
	        {
                var str = string.Format(TemplateXml, row.ToArray());
                var test = XmlDeserializeFromString <TestStandaloneXml>(str);
                tests.Add(test);
	        }

            return tests;
        }

        protected internal T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        protected object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }



    }
}
