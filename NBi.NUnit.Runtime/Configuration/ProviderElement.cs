using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Configuration
{
    public class ProviderElement : ConfigurationElement
    {
        [ConfigurationProperty("id",
            IsRequired = true,
            IsKey = true)]
        public string Id
        {
            get => (string)this["id"];
            set => this["id"] = value;
        }

        [ConfigurationProperty("invariant-name",
            IsRequired = true)]
        public string InvariantName
        {
            get => (string)this["invariant-name"];
            set => this["invariant-name"] = value;
        }


        // Constructor allowing name, url, and port to be specified. 
        public ProviderElement(string id, string invariantName)
        {
            Id = id;
            InvariantName = invariantName;
        }

        public ProviderElement()
        { }

        // Constructor allowing name to be specified, will take the 
        // default values for url and port. 
        public ProviderElement(string elementId)
        {
            Id = elementId;
        }


    }
}
