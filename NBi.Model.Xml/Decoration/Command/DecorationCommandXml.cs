using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Model.Xml.Settings;
using NBi.Model.Xml.Constraints;
using System.Collections.Generic;

namespace NBi.Model.Xml.Decoration.Command;

public abstract class DecorationCommandXml
{
    [XmlIgnore()]
    public Guid Guid { get; } = Guid.NewGuid();

    [XmlIgnore()]
    public virtual SettingsXml Settings { get; set; }

    [XmlIgnore()]
    public virtual DefaultXml Default
    {
        get
        {
            return Settings.GetDefault(SettingsXml.DefaultScope.Decoration);
        }
    }

}
