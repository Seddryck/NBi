using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Antlr4.StringTemplate;
using NBi.Xml;
using NBi.Xml.SerializationOption;

namespace NBi.Service
{
    public class StringTemplateEngine
    {
        public string TemplateXml { get; private set; }
        protected Template Template { get; private set; }
        public string PreProcessedTemplate { get; private set; }
        public string[] Variables { get; private set; }

        public StringTemplateEngine(string templateXml, string[] variables)
        {
            TemplateXml = templateXml;
            Variables = variables;
        }

        public IEnumerable<TestXml> Build(List<List<List<object>>> table)
        {
            InitializeTemplate();
            var tests = new List<TestXml>();

            //For each row, we need to fill the variables and render the template. 
            int count=0;
            foreach (var row in table)
            {
                count++;
                var str = BuildTestString(row);

                TestStandaloneXml test = null;
                try
                {
                    test = XmlDeserializeFromString<TestStandaloneXml>(str);
                }
                catch (InvalidOperationException ex)
                {
                    throw new TemplateExecutionException(ex.Message);
                }

                //Cleanup the variables in the template for next iteration.
                foreach (var variable in Variables)
                    Template.Remove(variable);

                test.Content = XmlSerializeFrom<TestStandaloneXml>(test);
                tests.Add(test);
                InvokeProgress(new ProgressEventArgs(count, table.Count()));
            }
            
            return tests;
        }

        internal void InitializeTemplate()
        {
            var group = new TemplateGroup('$', '$');
            group.RegisterRenderer(typeof(string), new StringRenderer());
            Template = new Template(group, TemplateXml);

            //Populate the "dynamic" variables once (The value is the same for all render that will occur).
            var dynNames = GetDynamicNames();
            var dynValues = GetDynamicValues();
            for (int i = 0; i < dynNames.Count(); i++)
                Template.Add(dynNames[i], dynValues[i]);
        }

        internal string BuildTestString(List<List<object>> values)
        {
            for (int i = 0; i < Variables.Count(); i++)
            { 
                // If the variable is not initialized or if it's value is "(none)" then we skip it.
                if (!(values[i].Count() == 0 || (values[i].Count == 1 && (values[i][0].ToString() == "(none)" || values[i][0].ToString()==string.Empty))))
                    Template.Add(Variables[i], values[i]);
                else
                    Template.Add(Variables[i], null);      
            }

            var str = Template.Render();
            return str;
        }

        protected internal T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        protected internal static string XmlSerializeFrom<T>(T objectData)
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

        protected static string SerializeFrom(object objectData, Type type)
        {
            var overrides = new WriteOnlyAttributes();
            overrides.Build();

            var serializer = new XmlSerializer(type, overrides);
            var result = string.Empty;
            using (var writer = new StringWriter())
            {
                // Use the Serialize method to store the object's state.
                try
                {
                    serializer.Serialize(writer, objectData);
                }
                catch (Exception e)
                {
                    
                    throw e;
                }
                
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
            return GetDynamicValues
            (
                DateTime.Now,
                (++uid),
                Environment.UserName
            );
        }

        protected virtual string[] GetDynamicValues(DateTime now, int uid, string userName)
        {
            return new string[]
            {
                String.Format("{0}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}",now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second),
                now.ToLongTimeString(),
                now.Date.ToShortDateString(),
                (uid).ToString(),
                userName
            };
        }

        public event EventHandler<ProgressEventArgs> Progressed;
        public void InvokeProgress(ProgressEventArgs e)
        {
            var handler = Progressed;
            if (handler != null)
                handler(this, e);
        }

    }
}
