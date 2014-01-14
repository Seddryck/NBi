using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NBi.Service.Dto;
using NBi.Xml.Settings;

namespace NBi.Service
{
    public class SettingsManager
    {
        private const string DefaultSutName = "Default - System-under-test";
        private const string DefaultAssertName = "Default - Assert";
        private const string ReferenceFormatName = "Reference - {0}";

        public DefaultXml DefaultSut
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

        public DefaultXml DefaultAssert
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

        internal SettingsManager(Dictionary<string, string> dico)
        {
            Dictionary = dico;
        }

        public string GetValue(string name)
        {
            return Dictionary[name];
        }

        public void SetValue(string name, string value)
        {
            Dictionary[name]=value;
        }

        public bool Exists(string name)
        {
            return Dictionary.ContainsKey(name);
        }

        public string[] GetNames()
        {
            return Dictionary.Keys.ToArray();
        }

        internal SettingsXml Settings
        {
            get
            {
                var settings = new SettingsXml();
                if (DefaultSut!=null)
                    settings.Defaults.Add(DefaultSut);
                if (DefaultAssert != null)
                    settings.Defaults.Add(DefaultAssert);

                return settings;
            }
            set
            {
                Dictionary = new Dictionary<string, string>();
                Dictionary.Add(DefaultSutName, "");
                Dictionary.Add(DefaultAssertName, "");

                foreach (var settings in value.Defaults)
                {
                    switch (settings.ApplyTo)
                    {
                        case SettingsXml.DefaultScope.SystemUnderTest:
                            SetValue(DefaultSutName, settings.ConnectionString);
                            break;
                        case SettingsXml.DefaultScope.Assert:
                            SetValue(DefaultAssertName, settings.ConnectionString);
                            break;
                        default:
                            break;
                    }
                }
                foreach (var settings in value.References)
                {
                    Dictionary.Add(string.Format(ReferenceFormatName, settings.Name), settings.ConnectionString);
                }
                
            }
        }

        public void Add(string name, string value)
        {
            Dictionary.Add(string.Format(ReferenceFormatName, name), value);
        }

        public void Remove(string name)
        {
            Dictionary.Remove(name);
        }

        public bool IsValidReferenceName(string name)
        {
            string strTheseAreInvalidFileNameChars = new string(System.IO.Path.GetInvalidFileNameChars()) + " ";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            return !containsABadCharacter.IsMatch(name);
        }

        public bool IsReferenceSelected(string name)
        {
            return !string.IsNullOrEmpty(name) && name.StartsWith("Reference - ");
        }

        public IEnumerable<Setting> GetSettings()
        {
            var list = new List<Setting>();
            foreach (var key in Dictionary.Keys.ToArray())
            {
                var setting = new Setting()
                {
                    Name = key,
                    Value = Dictionary[key]
                };
                list.Add(setting);
            }

            return list;
        }
    }
}
