using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml.Constraints
{
    public class ResultSetEquivalentToXml : EqualToXml
    {
        internal ResultSetEquivalentToXml()
            : base() { }

        internal ResultSetEquivalentToXml(SettingsXml settings)
            : base(settings) { }

        internal ResultSetEquivalentToXml(bool parallelizeQueries)
            : base(parallelizeQueries) { }
    }
}
