using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Configuration
{
    public class ExtensionElement : ConfigurationElement
    {
        public IDictionary<string, string> Parameters { get; private set; } = new Dictionary<string, string>();

        public ExtensionElement(string assembly)
        {
            Assembly = assembly;
        }

        public ExtensionElement()
        { }

        [ConfigurationProperty("assembly",
            IsRequired = true,
            IsKey = true)]
        public string Assembly
        {
            get => (string)this["assembly"];
            set =>this["assembly"] = value;
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            Parameters.Add(name, value);
            return true;
        }

    }
}
