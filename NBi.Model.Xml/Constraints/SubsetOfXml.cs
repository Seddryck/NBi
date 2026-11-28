using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Settings;
using NBi.Model.Xml.Systems;

namespace NBi.Model.Xml.Constraints;

public class SubsetOfXml : EqualToXml
{
    internal SubsetOfXml()
        : base() { }

    internal SubsetOfXml(SettingsXml settings)
        : base(settings) { }

    internal SubsetOfXml(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
