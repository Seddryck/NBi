using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Runtime.Configuration
{
    public class VariableCollection : ConfigurationElementCollection
    {

        public VariableCollection()
        { }

        public override ConfigurationElementCollectionType CollectionType
        {
            get => ConfigurationElementCollectionType.AddRemoveClearMap;
        }

        protected override ConfigurationElement CreateNewElement()
            => new VariableElement();

        protected override ConfigurationElement CreateNewElement(string name)
            => new VariableElement(name);

        protected override Object GetElementKey(ConfigurationElement element)
            => ((VariableElement)element).Name;

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

        public VariableElement this[int index]
        {
            get => (VariableElement)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public VariableElement this[string Name]
        {
            get => (VariableElement)BaseGet(Name);
        }

        public int IndexOf(VariableElement name) => BaseIndexOf(name);

        public void Add(VariableElement name) => BaseAdd(name);

        protected override void BaseAdd(ConfigurationElement element) => BaseAdd(element, false);

        public void Remove(VariableElement assembly)
        {
            if (BaseIndexOf(assembly) >= 0)
                BaseRemove(assembly);
        }

        public void RemoveAt(int index) => BaseRemoveAt(index);

        public void Remove(string name) => BaseRemove(name);

        public void Clear() => BaseClear();
    }
}
