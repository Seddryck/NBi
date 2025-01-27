using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Variables;
using System.Collections.Generic;
using NBi.Core.Variable;

namespace NBi.Xml.Constraints;

public abstract class AbstractConstraintXml
{
    private DefaultXml? _default;
    [XmlIgnore()]
    public virtual DefaultXml? Default
    {
        get { return _default; }
        set
        {
            _default = value;
            if (BaseItem != null)
                BaseItem.Default = value;
        }
    }
    private SettingsXml? settings;
    [XmlIgnore()]
    public virtual SettingsXml? Settings
    {
        get { return settings; }
        set
        {
            settings = value;
            if (BaseItem != null)
                BaseItem.Settings = value;
        }
    }

    [XmlIgnore]
    public virtual BaseItem? BaseItem
        => null;

    [XmlAttribute("not")]
    [DefaultValue(false)]
    public bool Not { get; set; }
}
