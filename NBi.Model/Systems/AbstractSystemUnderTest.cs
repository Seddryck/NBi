using System;
using System.Collections.Generic;
using NBi.Model.Items;
using NBi.Model.Settings;

namespace NBi.Model.Systems;

public abstract class AbstractSystemUnderTest
{
    public string? Name { get; set; }

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

    public abstract BaseItem? BaseItem { get; }

    public AbstractSystemUnderTest()
    {
        Default = new Default();
        Settings = new SuiteSettings();
    }

    internal virtual Dictionary<string, string> GetRegexMatch()
    {
        return new Dictionary<string, string>();
    }

    public abstract ICollection<string> GetAutoCategories();
}
