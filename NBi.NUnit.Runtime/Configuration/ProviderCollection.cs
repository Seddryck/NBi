using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Configuration
{
    public class ProviderCollection : ConfigurationElementCollection
    {

        public ProviderCollection()
        {
            
        }

        public override 
            ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return 

                    ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override 
            ConfigurationElement CreateNewElement()
        {
            return new ProviderElement();
        }


        protected override 
            ConfigurationElement CreateNewElement(
            string elementName)
        {
            return new ProviderElement(elementName);
        }


        protected override Object 
            GetElementKey(ConfigurationElement element)
        {
            return ((ProviderElement)element).Id;
        }


        public new string AddElementName
        {
            get
            { return base.AddElementName; }

            set
            { base.AddElementName = value; }

        }

        public new string ClearElementName
        {
            get
            { return base.ClearElementName; }

            set
            { base.ClearElementName = value; }

        }

        public new string RemoveElementName
        {
            get
            { return base.RemoveElementName; }
        }

        public new int Count
        {
            get { return base.Count; }
        }


        public ProviderElement this[int index]
        {
            get
            {
                return (ProviderElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public ProviderElement this[string Name]
        {
            get
            {
                return (ProviderElement)BaseGet(Name);
            }
        }

        public int IndexOf(ProviderElement url)
        {
            return BaseIndexOf(url);
        }

        public void Add(ProviderElement url)
        {
            BaseAdd(url);
            // Add custom code here.
        }

        protected override void 
            BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
            // Add custom code here.
        }

        public void Remove(ProviderElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Id);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
            // Add custom code here.
        }

        public Dictionary<string, string> ToDictionary()
        {
            var dico = new Dictionary<string, string>();
            foreach (var item in this)
	        {
                var provider = (ProviderElement)item;
                dico.Add(provider.Id, provider.InvariantName);
	        }
            return dico;
        }
    }
}
