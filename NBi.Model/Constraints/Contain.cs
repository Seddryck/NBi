using System;
using System.Collections.Generic;
using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Systems;

namespace NBi.Model.Constraints;

public class Contain : AbstractConstraintForCollection
{
    [Obsolete("Use Items in place of Caption")]
    public string Caption
    {
        get
        {
            if (Items.Count==1)
                return Items[0];
            throw new InvalidOperationException();
        }
        set
        {
            if (Items.Count == 0)
                Items.Add(value);
            else if (Items.Count == 1)
                Items[0] = value;
            else
                throw new InvalidOperationException();
        }
    }

}
