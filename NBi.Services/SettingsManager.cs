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
        private SettingsXml settings;

        private const string DefaultSutName = "Default - System-under-test";
        private const string DefaultAssertName = "Default - Assert";
        private const string ReferenceFormatName = "Reference - {0}";
        
        
        public SettingsManager()
        {
            settings = new SettingsXml();
        }
        

        public void SetValue(string name, string value)
        {
            switch (name)
            {
                case DefaultSutName:
                    settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionString = value;
                    return;
                case DefaultAssertName:
                    settings.GetDefault(SettingsXml.DefaultScope.Assert).ConnectionString = value;
                    return;
            }

            if (!name.StartsWith("Reference"))
                throw new ArgumentException();

            var refName = name.Split(new[] { '-' })[1].Trim();
            var reference = settings.References.SingleOrDefault(r => r.Name == refName);
            if (reference==null)
                throw new ArgumentException();

            reference.ConnectionString = value;

        }

        public bool Exists(string name)
        {
            return settings.References.SingleOrDefault(r => r.Name == name) != null;
        }
        
        public void SetParallelizeQueries(bool value)
        {
            settings.ParallelizeQueries = value;
        }

        public SettingsXml GetSettingsXml()
        {
            return settings;
        }

        public void SetSettingsXml(SettingsXml settings)
        {
            this.settings = settings;
        }

        public void Add(string name, string value)
        {
            var reference = settings.References.SingleOrDefault(r => r.Name == name);
            if (reference != null)
                throw new ArgumentException();

            settings.References.Add(new ReferenceXml() { Name = name, ConnectionString = value });
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
            list.Add(new Setting() { Name = DefaultSutName, Value = settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionString });
            list.Add(new Setting() { Name = DefaultAssertName, Value = settings.GetDefault(SettingsXml.DefaultScope.Assert).ConnectionString });

            foreach (var reference in settings.References)
            {
                var setting = new Setting()
                {
                    Name = string.Format(ReferenceFormatName, reference.Name),
                    Value = reference.ConnectionString
                };
                list.Add(setting);
            }

            return list;
        }

    }
}
