using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints
{
    public class ContainXml : AbstractConstraintForCollectionXml
    {
        [XmlIgnore]
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

        [XmlElement("members")]
        public MembersXml Members { get; set; }
    }
}
