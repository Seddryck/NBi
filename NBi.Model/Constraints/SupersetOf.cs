using System;
using System.Collections.Generic;
using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Systems;

namespace NBi.Model.Constraints;

public class SupersetOf : EqualTo
{
    public SupersetOf()
        : base() { }

    internal SupersetOf(SuiteSettings settings)
        : base(settings) { }

    internal SupersetOf(bool parallelizeQueries)
        : base(parallelizeQueries) { }
}
