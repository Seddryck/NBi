using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;

namespace NBi.Xml.Constraints
{
    public class ContainXml : AbstractConstraintXml
    {
        [XmlAttribute("ignore-case")]
        [DefaultValue(false)]
        public bool IgnoreCase { get; set; }

        [XmlAttribute("caption")]
        public string Caption 
        { 
            get
            {
                if (Items.Count==1)
                    return Items[0];
                throw new InvalidOperationException();
            }
            set
            {
                if (Items.Count == 0)
                    Items.Add(value);
                else if (Items.Count == 1)
                    Items[0] = value;
                else
                    throw new InvalidOperationException();
            }  
        }

        [XmlElement("item")]
        public List<string> Items { get; set; }

        [XmlElement("one-column-query")]
        public QueryXml Query { get; set; }

        public ContainXml()
        {
            Items = new List<string>();
        }

    }
}
