using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Settings;
using NBi.Model.Xml.Systems;

namespace NBi.Model.Xml.Constraints;

public class ContainXml : AbstractConstraintForCollectionXml
{
    [XmlIgnore]
    [Obsolete("Use Items in place of Caption")]
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

}
