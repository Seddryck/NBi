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
        private const string DefaultSetupCleanupName = "Default - Setup-cleanup";
        private const string DefaultEverywhereName = "Default - Everywhere";
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
                    settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionString.Inline = value;
                    return;
                case DefaultAssertName:
                    settings.GetDefault(SettingsXml.DefaultScope.Assert).ConnectionString.Inline = value;
                    return;
                case DefaultSetupCleanupName:
                    settings.GetDefault(SettingsXml.DefaultScope.Decoration).ConnectionString.Inline = value;
                    return;
                case DefaultEverywhereName:
                    settings.GetDefault(SettingsXml.DefaultScope.Everywhere).ConnectionString.Inline = value;
                    return;
            }

            if (!name.StartsWith("Reference"))
                throw new ArgumentException();

            var refName = name.Split(new[] { '-' })[1].Trim();
            var reference = settings.References.SingleOrDefault(r => r.Name == refName);
            if (reference == null)
                throw new ArgumentException();

            reference.ConnectionString.Inline = value;
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

            settings.References.Add(new ReferenceXml() { Name = name, ConnectionString = new ConnectionStringXml() { Inline = value } });
        }

        public bool IsValidReferenceName(string name)
        {
            string strTheseAreInvalidFileNameChars = new string(System.IO.Path.GetInvalidFileNameChars()) + " ";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            return !containsABadCharacter.IsMatch(name);
        }

        public IEnumerable<Setting> GetSettings()
        {
            var list = new List<Setting>()
            {

                new Setting() { Name = DefaultSutName, Value = settings.GetDefault(SettingsXml.DefaultScope.SystemUnderTest).ConnectionString.GetValue() },
                new Setting() { Name = DefaultAssertName, Value = settings.GetDefault(SettingsXml.DefaultScope.Assert).ConnectionString.GetValue() },
                new Setting() { Name = DefaultSetupCleanupName, Value = settings.GetDefault(SettingsXml.DefaultScope.Decoration).ConnectionString.GetValue() },
                new Setting() { Name = DefaultEverywhereName, Value = settings.GetDefault(SettingsXml.DefaultScope.Everywhere).ConnectionString.GetValue() }
            };

            foreach (var reference in settings.References)
            {
                var setting = new Setting()
                {
                    Name = string.Format(ReferenceFormatName, reference.Name),
                    Value = reference.ConnectionString.Inline
                };
                list.Add(setting);
            }

            return list;
        }

    }
}
