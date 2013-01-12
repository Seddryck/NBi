using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System;

namespace NBi.Xml.Settings
{
    public class SettingsXml
    {
        [XmlElement("default")]
        public List<DefaultXml> Defaults { get; set; }
        [XmlElement("reference")]
        public List<ReferenceXml> References { get; set; }

        public enum DefaultScope
        {
            [XmlEnum(Name = "system-under-test")]
            SystemUnderTest,
            [XmlEnum(Name = "assert")]
            Assert
        }

        public DefaultXml GetDefault(DefaultScope scope)
        {
            var def = Defaults.SingleOrDefault(d => d.ApplyTo == scope);
            //Don't throw exception ... generating lot of issues!
            //if (def == null)
            //    throw new ArgumentOutOfRangeException(string.Format("No default for '{0}' existing in test suite's settings.", Enum.GetName(typeof(DefaultScope), scope)));
            return def;           
        }

        public ReferenceXml GetReference(string name)
        {
            try
            {
                var reference = References.SingleOrDefault(d => d.Name == name);
                if (reference == null)
                    throw new ArgumentOutOfRangeException(string.Format("No reference named '{0}' existing in test suite's settings.", name));
                return reference;
            }
            catch (System.InvalidOperationException)
            {
                throw new InvalidOperationException(string.Format("All references'name must be unique in settings. The name '{0}' exists more than once.", name));
            }
            
        }

        public SettingsXml()
        {
            Defaults = new List<DefaultXml>();
            References = new List<ReferenceXml>();
        }
    }
}
