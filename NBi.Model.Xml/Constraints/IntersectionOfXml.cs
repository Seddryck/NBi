using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Model.Xml.Items;
using NBi.Model.Xml.Settings;
using NBi.Model.Xml.Systems;

namespace NBi.Model.Xml.Constraints;

public class IntersectionOfXml : EqualToXml
{
    internal IntersectionOfXml()
        : base() { }

    internal IntersectionOfXml(SettingsXml settings)
        : base(settings) { }

    internal IntersectionOfXml(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
