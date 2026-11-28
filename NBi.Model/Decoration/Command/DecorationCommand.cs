using System;
using System.Linq;
using NBi.Core;
using NBi.Model.Settings;
using NBi.Model.Constraints;
using System.Collections.Generic;

namespace NBi.Model.Decoration.Command;

public abstract class DecorationCommand
{
    public Guid Guid { get; } = Guid.NewGuid();

    public virtual SuiteSettings Settings { get; set; }

    public virtual Default Default
    {
        get
        {
            return Settings.GetDefault(SuiteSettings.DefaultScope.Decoration);
        }
    }
}
