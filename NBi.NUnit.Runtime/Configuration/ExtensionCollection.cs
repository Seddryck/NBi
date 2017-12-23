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
            return new ExtensionElement();
        }


        protected override 
            ConfigurationElement CreateNewElement(
            string elementName)
        {
            return new ExtensionElement(elementName);
        }


        protected override Object 
            GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionElement)element).Assembly;
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


        public ExtensionElement this[int index]
        {
            get
            {
                return (ExtensionElement)BaseGet(index);
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

        new public ExtensionElement this[string Name]
        {
            get
            {
                return (ExtensionElement)BaseGet(Name);
            }
        }

        public int IndexOf(ExtensionElement url)
        {
            return BaseIndexOf(url);
        }

        public void Add(ExtensionElement url)
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

        public void Remove(ExtensionElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Assembly);
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
    }
}
