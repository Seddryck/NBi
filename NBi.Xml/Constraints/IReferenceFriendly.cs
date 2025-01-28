using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Xml.Settings;

namespace NBi.Xml.Constraints;

interface IReferenceFriendly
{
    void AssignReferences(IEnumerable<ReferenceXml> references);
}
