using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;
using System.ComponentModel;
using NBi.Xml.Constraints;
using NBi.Xml.Settings;
using System.Reflection;

namespace NBi.Xml.Items
{
    public class EtlXml: ExecutableXml, IReferenceFriendly
    {
        protected const int DEFAULT_TIMEOUT = 30;
        protected const string DEFAULT_VERSION = "SqlServer2014";

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("server")]
        public string Server { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("username")]
        public string UserName { get; set; }

        [XmlAttribute("password")]
        public string Password { get; set; }

        [XmlAttribute("catalog")]
        public string Catalog { get; set; }

        [XmlAttribute("folder")]
        public string Folder { get; set; }

        [XmlAttribute("project")]
        public string Project { get; set; }

        [XmlAttribute("environment")]
        public string Environment { get; set; }

        [DefaultValue(false)]
        [XmlAttribute("bits-32")]
        public bool Is32Bits { get; set; }

        [DefaultValue(DEFAULT_TIMEOUT)]
        [XmlAttribute("timeout")]
        public int Timeout { get; set; }

        [XmlElement("parameter")]
        public List<EtlParameterXml> Parameters { get; set; } = new List<EtlParameterXml>();

        public EtlXml()
        {
            Version = DEFAULT_VERSION;
        }

        public void AssignReferences(IEnumerable<ReferenceXml> references)
        {
            var properties = typeof(EtlBaseXml).GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(string));

            foreach (var property in properties)
                AssignDefaultOrReference(property.Name, references);
        }

        private void AssignDefaultOrReference(string propertyName, IEnumerable<ReferenceXml> references)
        {
            if (this.GetType().GetProperty(propertyName).PropertyType==typeof(string))
            {
                var currentValue = (string)this.GetType().GetProperty(propertyName).GetValue(this, null);

                if (string.IsNullOrEmpty(currentValue))
                {
                    var defaultValue = typeof(EtlBaseXml).GetProperty(propertyName).GetValue(Default.Etl, null);
                    this.GetType().GetProperty(propertyName).SetValue(this, defaultValue);
                }
                else if (currentValue.StartsWith("@"))
                {
                    var refName = ((string)currentValue).Substring(1);
                    var refChoice = GetReference(references, refName);
                    if (refChoice.Etl == null)
                        throw new NullReferenceException(string.Format("A reference named '{0}' has been found, but no element 'etl' has been defined", refName));

                    var referenceValue = typeof(EtlBaseXml).GetProperty(propertyName).GetValue(refChoice.Etl, null);
                    this.GetType().GetProperty(propertyName).SetValue(this, referenceValue);
                }
            }
        }
        
        protected ReferenceXml GetReference(IEnumerable<ReferenceXml> references, string value)
        {
            if (references == null || references.Count() == 0)
                throw new InvalidOperationException("No reference has been defined for this constraint");

            var refChoice = references.FirstOrDefault(r => r.Name == value);
            if (refChoice == null)
                throw new IndexOutOfRangeException(string.Format("No reference named '{0}' has been defined.", value));
            return refChoice;
        }
    }
}
