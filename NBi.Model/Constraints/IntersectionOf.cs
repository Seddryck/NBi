using System;
using System.Collections.Generic;
using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Systems;

namespace NBi.Model.Constraints;

public class IntersectionOf : EqualTo
{
    internal IntersectionOf()
        : base() { }

    internal IntersectionOf(SuiteSettings settings)
        : base(settings) { }

    internal IntersectionOf(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
