using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;
using NBi.Xml.Settings;
using NBi.Xml.Constraints;
using System.Collections.Generic;

namespace NBi.Xml.Decoration.Command
{
    public abstract class DecorationCommandXml : IDecorationCommand
    {
        [XmlIgnore()]
        public virtual Settings.SettingsXml Settings { get; set; }

        [XmlIgnore()]
        public virtual Settings.DefaultXml Default
        {
            get
            {
                return Settings.GetDefault(Xml.Settings.SettingsXml.DefaultScope.Decoration);
            }
        }

    }
}
