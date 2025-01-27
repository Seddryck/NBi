using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints;

public class SubsetOfXml : EqualToXml
{
    internal SubsetOfXml()
        : base() { }

    internal SubsetOfXml(SettingsXml settings)
        : base(settings) { }

    internal SubsetOfXml(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
