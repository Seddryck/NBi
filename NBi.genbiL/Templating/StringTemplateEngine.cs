using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Antlr4.StringTemplate;
using NBi.Xml;
using NBi.Xml.SerializationOption;

namespace NBi.GenbiL.Templating
{
    public class StringTemplateEngine
    {
        private readonly IDictionary<Type, XmlSerializer> cacheSerializer;
        private readonly IDictionary<Type, XmlSerializer> cacheDeserializer;

        public string TemplateXml { get; private set; }
        protected Template Template { get; private set; }
        public string PreProcessedTemplate { get; private set; }
        public string[] Variables { get; private set; }

        public StringTemplateEngine(string templateXml, string[] variables)
        {
            TemplateXml = templateXml;
            Variables = variables;
            cacheSerializer = new Dictionary<Type, XmlSerializer>();
            cacheDeserializer = new Dictionary<Type, XmlSerializer>();
        }

        public IEnumerable<TestStandaloneXml> BuildTests(List<List<List<object>>> table, IDictionary<string, object> consumables)
            => Build<TestStandaloneXml>(table, consumables);

        protected internal IEnumerable<T> Build<T>(List<List<List<object>>> table, IDictionary<string, object> consumables)
        {
            InitializeTemplate(consumables);

            //For each row, we need to fill the variables and render the template. 
            int count = 0;
            foreach (var row in table)
            {
                count++;
                var str = RenderTemplate(row);

                //Cleanup the variables in the template for next iteration.
                foreach (var variable in Variables)
                    Template.Remove(variable);

                var obj = (typeof(T) == typeof(string)) ? (T)Convert.ChangeType(str, typeof(T)) : Deserialize<T>(str);
                if (obj is TestStandaloneXml)
                    (obj as TestStandaloneXml).Content = XmlSerializeFrom(obj as TestStandaloneXml);
                InvokeProgress(new ProgressEventArgs(count, table.Count()));
                yield return obj;
            }
        }

        protected virtual T Deserialize<T>(string value)
        {
            T obj;
            try
            { obj = XmlDeserializeFromString<T>(value); }
            catch (InvalidOperationException ex)
            { throw new TemplateExecutionException(ex.Message); }
            return obj;
        }

        internal void InitializeTemplate(IDictionary<string, object> consumables)
        {
            var group = new TemplateGroup('$', '$');
            group.RegisterRenderer(typeof(string), new StringRenderer());
            Template = new Template(group, TemplateXml);

            //Add all the global variables (not defined in a scope)
            if (consumables != null)
                foreach (var variable in consumables)
                    Template.Add(variable.Key, variable.Value);
        }

        internal string RenderTemplate(List<List<object>> values)
        {
            for (int i = 0; i < Variables.Count(); i++)
            {
                // If the variable is not initialized or if it's value is "(none)" then we skip it.
                if (!(values[i].Count() == 0 || (values[i].Count == 1 && (values[i][0].ToString() == "(none)" || values[i][0].ToString() == string.Empty))))
                    Template.Add(Variables[i], values[i]);
                else
                    Template.Add(Variables[i], null);
            }

            var str = Template.Render();
            return str;
        }

        protected internal T XmlDeserializeFromString<T>(string objectData)
            => (T)XmlDeserializeFromString(objectData, typeof(T));

        protected internal string XmlSerializeFrom<T>(T objectData)
            => SerializeFrom(objectData, typeof(T));


        protected object XmlDeserializeFromString(string objectData, Type type)
        {
            if (!cacheDeserializer.ContainsKey(type))
            {
                var overrides = new ReadOnlyAttributes();
                overrides.Build();
                var builtDeserializer = new XmlSerializer(type, overrides);
                cacheDeserializer.Add(type, builtDeserializer);
            }

            var serializer = cacheDeserializer[type];
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        protected string SerializeFrom(object objectData, Type type)
        {
            if (!cacheSerializer.ContainsKey(type))
            {
                var overrides = new WriteOnlyAttributes();
                overrides.Build();
                var builtSerializer = new XmlSerializer(type, overrides);
                cacheSerializer.Add(type, builtSerializer);
            }

            var serializer = cacheSerializer[type];

            var result = string.Empty;
            using (var writer = new StringWriter())
            {
                // Use the Serialize method to store the object's state.
                try
                { serializer.Serialize(writer, objectData); }
                catch (Exception e)
                { throw e; }

                result = writer.ToString();
            }
            return result;
        }

        public event EventHandler<ProgressEventArgs> Progressed;
        public void InvokeProgress(ProgressEventArgs e)
        {
            Progressed?.Invoke(this, e);
        }

    }
}
