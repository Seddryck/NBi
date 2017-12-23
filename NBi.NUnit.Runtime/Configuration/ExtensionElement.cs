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
        [ConfigurationProperty("assembly",
            IsRequired = true,
            IsKey = true)]
        public string Assembly
        {
            get
            {
                return (string)this["assembly"];
            }
            set
            {
                this["assembly"] = value;
            }
        }


        // Constructor allowing name, url, and port to be specified. 
        public ExtensionElement(string assembly)
        {
            Assembly = assembly;
        }

        public ExtensionElement()
        {

        }
    }
}
