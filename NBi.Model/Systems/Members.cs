using System;
using System.Collections.Generic;
using NBi.Model.Items;

namespace NBi.Model.Systems;

public class Members : AbstractSystemUnderTest
{
    public Members() : base()
    {
        Exclude = new Exclude();
    }

    public string ChildrenOf { get; set; }

    public AbstractMembersItem Item { get; set; }

    public Exclude Exclude { get; set; }

    public bool ExcludeSpecified
    {
        get
        {
            return !(
                        Exclude == null
                        || (
                                (Exclude.Items==null || Exclude.Items.Count == 0)
                                && (Exclude.Patterns==null || Exclude.Patterns.Count == 0)
                            )
                     );
        }
        set { return; }
    }

    public override BaseItem BaseItem
    {
        get
        {
            return Item;
        }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        return Item.GetRegexMatch();
    }

    public override ICollection<string> GetAutoCategories()
    {
        var values = Item.GetAutoCategories();
        values.Add("Members");
        return values;
    }
}
