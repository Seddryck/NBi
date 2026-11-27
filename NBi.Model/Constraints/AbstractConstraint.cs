using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Variables;
using System.Collections.Generic;
using NBi.Core.Variable;

namespace NBi.Model.Constraints;

public abstract class AbstractConstraint
{
    private Default? _default;
    public virtual Default? Default
    {
        get { return _default; }
        set
        {
            _default = value;
            if (BaseItem != null)
                BaseItem.Default = value;
        }
    }
    private SuiteSettings? settings;
    public virtual SuiteSettings? Settings
    {
        get { return settings; }
        set
        {
            settings = value;
            if (BaseItem != null)
                BaseItem.Settings = value;
        }
    }

    public virtual BaseItem? BaseItem
        => null;

    public bool Not { get; set; }
}
