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
        private const string DefaultEverywhereName = "Default - Everywhere";
        private const string DefaultSutName = "Default - System-under-test";
        private const string DefaultAssertName = "Default - Assert";
        private const string DefaultSetupCleanupName = "Default - Setup-cleanup";
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

        public DefaultXml DefaultEverywhere
        {
            get
            {
                if (string.IsNullOrEmpty(Dictionary[DefaultEverywhereName]))
                    return null;

                return new DefaultXml()
                {
                    ApplyTo = SettingsXml.DefaultScope.Everywhere,
                    ConnectionString = Dictionary[DefaultEverywhereName]
                };
            }
        }

        public DefaultXml DefaultSetupCleanup
        {
            get
            {
                if (string.IsNullOrEmpty(Dictionary[DefaultSetupCleanupName]))
                    return null;

                return new DefaultXml()
                {
                    ApplyTo = SettingsXml.DefaultScope.Decoration,
                    ConnectionString = Dictionary[DefaultSetupCleanupName]
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

        private bool parallelizeQueries;
        public void SetParallelizeQueries(bool value)
        {
            parallelizeQueries = value;
        }

        public SettingsXml GetSettingsXml()
        {
            var settings = new SettingsXml();
            settings.ParallelizeQueries = parallelizeQueries;
            if (DefaultSut!=null)
                settings.Defaults.Add(DefaultSut);
            if (DefaultAssert != null)
                settings.Defaults.Add(DefaultAssert);

            foreach (var s in Dictionary)
            {
                if (!s.Key.StartsWith("Default"))
                    settings.References.Add(new ReferenceXml() { Name = s.Key.Split(' ')[2], ConnectionString = s.Value });
            }

            return settings;
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
