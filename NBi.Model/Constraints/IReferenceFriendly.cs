using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Model.Settings;

namespace NBi.Model.Constraints;

interface IReferenceFriendly
{
    void AssignReferences(IEnumerable<Reference> references);
}
