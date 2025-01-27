using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints;

public class IntersectionOfXml : EqualToXml
{
    internal IntersectionOfXml()
        : base() { }

    internal IntersectionOfXml(SettingsXml settings)
        : base(settings) { }

    internal IntersectionOfXml(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
