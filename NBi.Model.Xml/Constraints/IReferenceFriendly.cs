using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Model.Xml.Settings;

namespace NBi.Model.Xml.Constraints;

interface IReferenceFriendly
{
    void AssignReferences(IEnumerable<ReferenceXml> references);
}
