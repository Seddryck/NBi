using System;
using System.Collections.Generic;
using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Systems;

namespace NBi.Model.Constraints;

public class SubsetOf : EqualTo
{
    internal SubsetOf()
        : base() { }

    internal SubsetOf(SuiteSettings settings)
        : base(settings) { }

    internal SubsetOf(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
