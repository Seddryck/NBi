using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml.Decoration.Check
{
    public abstract class DecorationCheckXml : IDecorationCheck
    {
        [XmlIgnore()]
        public Settings.SettingsXml Settings { get; set; }
    }
}
