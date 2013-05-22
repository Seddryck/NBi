using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Xml.Settings;

namespace NBi.Service
{
    public class SettingsManager
    {
        private const string DefaultSutName = "Default - System-under-test";
        private const string DefaultAssertName = "Default - Assert";

        protected DefaultXml DefaultSut
        {
            get
            {
                if (string.IsNullOrEmpty(Dictionary[DefaultSutName]))
                    return null;

                return new DefaultXml() 
                    { 
                        ApplyTo = SettingsXml.DefaultScope.SystemUnderTest,
                        ConnectionString = Dictionary[DefaultSutName]
                    };
            }
        }

        protected DefaultXml DefaultAssert
        {
            get
            {
                if (string.IsNullOrEmpty(Dictionary[DefaultAssertName]))
                    return null;

                return new DefaultXml()
                {
                    ApplyTo = SettingsXml.DefaultScope.Assert,
                    ConnectionString = Dictionary[DefaultAssertName]
                };
            }
        }

        protected Dictionary<string, string> Dictionary {get; set;}

        public SettingsManager()
        {
            Dictionary = new Dictionary<string, string>();
            Dictionary.Add(DefaultSutName, "");
            Dictionary.Add(DefaultAssertName, "");
        }

        public string GetValue(string name)
        {
            return Dictionary[name];
        }

        public void SetValue(string name, string value)
        {
            Dictionary[name]=value;
        }

        public string[] GetNames()
        {
            return Dictionary.Keys.ToArray();
        }

        public Xml.Settings.SettingsXml GetSettings()
        {
            var settings = new SettingsXml();
            if (DefaultSut!=null)
                settings.Defaults.Add(DefaultSut);
            if (DefaultAssert != null)
                settings.Defaults.Add(DefaultAssert);

            return settings;
        }
    }
}
