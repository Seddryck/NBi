using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints;

public class SupersetOfXml : EqualToXml
{
    public SupersetOfXml()
        : base() { }

    internal SupersetOfXml(SettingsXml settings)
        : base(settings) { }

    internal SupersetOfXml(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
