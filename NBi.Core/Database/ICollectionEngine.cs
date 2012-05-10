using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    public interface ICollectionEngine
    {
        Result Validate(ICollection coll);

        int? Exactly { get; set; }
        int? MoreThan { get; set; }
        int? LessThan { get; set; }
    }
}
