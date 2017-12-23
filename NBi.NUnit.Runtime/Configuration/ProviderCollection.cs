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
        { }

        public override ConfigurationElementCollectionType CollectionType
        {
            get => ConfigurationElementCollectionType.AddRemoveClearMap;
        }

        protected override ConfigurationElement CreateNewElement()
            => new ProviderElement();

        protected override ConfigurationElement CreateNewElement(string elementName)
            =>new ProviderElement(elementName);

        protected override Object GetElementKey(ConfigurationElement element)
            => ((ProviderElement)element).Id;

        public new string AddElementName
        {
            get => base.AddElementName;
            set => base.AddElementName = value;
        }

        public new string ClearElementName
        {
            get => base.ClearElementName;
            set => base.ClearElementName = value;

        }

        public new string RemoveElementName
        {
            get => base.RemoveElementName;
        }

        public new int Count
        {
            get => base.Count;
        }


        public ProviderElement this[int index]
        {
            get => (ProviderElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public ProviderElement this[string Name]
        {
            get => (ProviderElement)BaseGet(Name);
        }

        public int IndexOf(ProviderElement url) => BaseIndexOf(url);

        public void Add(ProviderElement url) => BaseAdd(url);

        protected override void BaseAdd(ConfigurationElement element) => BaseAdd(element, false);

        public void Remove(ProviderElement url)
        {
            if (BaseIndexOf(url) >= 0)
                BaseRemove(url.Id);
        }

        public void RemoveAt(int index) => BaseRemoveAt(index);

        public void Remove(string name) => BaseRemove(name);

        public void Clear() => BaseClear();

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
