using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Antlr4.StringTemplate;
using NBi.Xml;

namespace NBi.Service
{
    public class StringTemplateEngine
    {
        public string TemplateXml { get; private set; }
        public string PreProcessedTemplate { get; private set; }
        public string[] Variables { get; private set; }

        public StringTemplateEngine(string templateXml, string[] variables)
        {
            TemplateXml = templateXml;
            Variables = variables;
        }

        public IEnumerable<TestXml> Build(List<List<List<object>>> table)
        {
            var tests = new List<TestXml>();

            foreach (var row in table)
            {
                Template template = new Template(TemplateXml, '$', '$');
                for (int i = 0; i < Variables.Count(); i++)
                    template.Add(Variables[i], row[i]);

                var dynNames = GetDynamicNames();
                var dynValues = GetDynamicValues();
                for (int i = 0; i < dynNames.Count(); i++)
                    template.Add(dynNames[i], dynValues[i]);

                var str = template.Render();

                var test = XmlDeserializeFromString<TestStandaloneXml>(str);
                test.Content = XmlSerializeFrom<TestStandaloneXml>(test);
                tests.Add(test);
            }
            
            return tests;
        }

        protected internal T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        protected internal string XmlSerializeFrom<T>(T objectData)
        {
            return SerializeFrom(objectData, typeof(T));
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

        protected string SerializeFrom(object objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            var result = string.Empty;
            using (var writer = new StringWriter())
            {
                // Use the Serialize method to store the object's state.
                serializer.Serialize(writer, objectData);
                result = writer.ToString();
            }
            return result;
        }

        protected string[] GetDynamicNames()
        {
            return new string []
            {
                "now",
                "time",
                "today",
                "uid",
                "username"
            };
        }

        private int uid=0;
        protected string[] GetDynamicValues()
        {
            return new string[]
            {
                String.Format("{0}-{1:00}-{2:00}T{3}",DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.ToLongTimeString()),
                DateTime.Now.ToLongTimeString(),
                DateTime.Today.ToShortDateString(),
                (++uid).ToString(),
                Environment.UserName
            };
        }



    }
}
