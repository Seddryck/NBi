using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Configuration
{
    public class ExtensionCollection : ConfigurationElementCollection
    {
        public ExtensionCollection()
        { }

        public override ConfigurationElementCollectionType CollectionType
            { get => ConfigurationElementCollectionType.AddRemoveClearMap; }

        protected override ConfigurationElement CreateNewElement()
            => new ExtensionElement();

        protected override ConfigurationElement CreateNewElement(string elementName)
            => new ExtensionElement(elementName);

        protected override Object GetElementKey(ConfigurationElement element)
            => ((ExtensionElement)element).Assembly;

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

        public new string RemoveElementName { get => base.RemoveElementName; }

        public new int Count { get => base.Count; }

        public ExtensionElement this[int index]
        {
            get => (ExtensionElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public ExtensionElement this[string Name] { get => (ExtensionElement)BaseGet(Name); }

        public int IndexOf(ExtensionElement assembly) => BaseIndexOf(assembly);

        public void Add(ExtensionElement assembly) => BaseAdd(assembly);

        protected override void BaseAdd(ConfigurationElement element) => BaseAdd(element, false);

        public void Remove(ExtensionElement assembly)
        {
            if (BaseIndexOf(assembly) >= 0)
                BaseRemove(assembly);
        }

        public void RemoveAt(int index) => BaseRemoveAt(index);

        public void Remove(string name) => BaseRemove(name);

        public void Clear() => BaseClear();
    }
}
