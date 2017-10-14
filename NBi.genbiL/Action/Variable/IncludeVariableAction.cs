using NBi.Core.Variable;
using NBi.Xml;
using NBi.Xml.Settings;
using NBi.Xml.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.GenbiL.Action.Variable
{
    public class IncludeVariableAction : IVariableAction
    {
        public string Filename { get; set; }

        public IncludeVariableAction(string filename)
        {
            Filename = filename;
        }

        public void Execute(GenerationState state)
        {
                var variables = ReadXml(Filename);
                var factory = new TestVariableFactory();

                foreach (var variable in variables)
                    state.Variables.Add(variable.Name, factory.Instantiate(variable.Script.Language, variable.Script.Code));
        }

        protected virtual IEnumerable<GlobalVariableXml> ReadXml(string filename)
        {
            using (var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read))
                return ReadXml(stream);
        }

        protected internal IEnumerable<GlobalVariableXml> ReadXml(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var str = reader.ReadToEnd();
                var standalone = XmlDeserializeFromString<GlobalVariablesStandaloneXml>(str);
                var globalVariables = new List<GlobalVariableXml>();
                globalVariables = standalone.Variables;
                return globalVariables;
            }
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

        public string Display
        {
            get
            {
                return string.Format("Include variables from '{0}'"
                    , Filename
                    );
            }
        }
    }
}
