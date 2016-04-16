using NBi.Xml;
using NBi.Xml.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.GenbiL.Action.Setting
{
    public class IncludeSettingAction : ISettingAction
    {
        public string Filename { get; set; }

        public IncludeSettingAction(string filename)
        {
            Filename = filename;
        }

        public void Execute(GenerationState state)
        {
            using (var stream = new FileStream(Filename, FileMode.Open, FileAccess.Read))
            {
                var settings = Include(stream);
                state.Settings.SetSettingsXml(settings);
            }
        }

        protected internal SettingsXml Include(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var str = reader.ReadToEnd();
                var standalone = XmlDeserializeFromString<SettingsStandaloneXml>(str);
                var settings = new SettingsXml();
                settings.Defaults = standalone.Defaults;
                settings.References = standalone.References;
                settings.ParallelizeQueries = standalone.ParallelizeQueries;
                settings.CsvProfile = standalone.CsvProfile;
                return settings;
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
                return string.Format("Include settings from '{0}'"
                    , Filename
                    );
            }
        }
    }
}
