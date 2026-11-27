using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Settings;
using NBi.Model.Xml.Systems;

namespace NBi.Model.Xml.Constraints;

public class SupersetOfXml : EqualToXml
{
    public SupersetOfXml()
        : base() { }

    internal SupersetOfXml(SettingsXml settings)
        : base(settings) { }

    internal SupersetOfXml(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
