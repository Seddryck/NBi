using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using NBi.Xml;

namespace NBi.Service
{
    public class GenericTestMaker
    {
        public string TemplateXml { get; private set; }
        public string PreProcessedTemplate { get; private set; }
        public string[] Variables { get; private set; }

        public GenericTestMaker(string templateXml, string[] variables)
        {
            TemplateXml = templateXml;
            Variables = variables;
        }

        protected internal string PreProcess(string templateXml, string[] variables, string[] dynamics)
        {
            var preProcessed = templateXml;
            var i = 0;
            foreach (var variable in variables)
            {
                preProcessed = preProcessed.Replace("{" + variable + "}", "{" + i.ToString() + "}");
                i++;
            }

            foreach (var dyn in dynamics)
            {
                preProcessed = preProcessed.Replace("{@@" + dyn + "}", "{" + i.ToString() + "}");
                i++;
            }

            return preProcessed;
        }

        public IEnumerable<TestXml> Build(IEnumerable<IList<string>> rows)
        {
            if (string.IsNullOrEmpty(PreProcessedTemplate))
                PreProcessedTemplate = PreProcess(TemplateXml, Variables, GetDynamicNames());
            
            var tests = new List<TestXml>();

            foreach (var row in rows)
	        {
                var rowList = row.ToList();
                rowList.AddRange(GetDynamicValues());
                var str = string.Format(PreProcessedTemplate, rowList.ToArray());
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
        protected IEnumerable<string> GetDynamicValues()
        {
            return new string[]
            {
                DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), 
                DateTime.Now.ToLongTimeString(),
                DateTime.Today.ToShortDateString(),
                (++uid).ToString(),
                Environment.UserName
            };
        }



    }
}
