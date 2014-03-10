﻿using System;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core;

namespace NBi.Xml.Decoration.Condition
{
    public abstract class DecorationConditionXml : IDecorationCheck
    {
        [XmlIgnore()]
        public Settings.SettingsXml Settings { get; set; }
    }
}
