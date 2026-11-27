using System.Collections.Generic;
using NBi.Model.Items;
using NBi.Model.Settings;
using NBi.Model.Constraints;

namespace NBi.Model.Systems;

public class Execution : AbstractSystemUnderTest, IReferenceFriendly
{
    public virtual bool IsQuery()
    {
        return true;
    }

    public virtual Executable Item { get; set; }

    public override BaseItem BaseItem
    {
        get
        {
            return (BaseItem) Item;
        }
    }

    internal override Dictionary<string, string> GetRegexMatch()
    {
        var dico = base.GetRegexMatch();
        return dico;
    }

    public override ICollection<string> GetAutoCategories()
    {
        return new string[] { "Execution" };
    }

    public void AssignReferences(IEnumerable<Reference> references)
    {
        if (Item is IReferenceFriendly)
            ((IReferenceFriendly)Item).AssignReferences(references);
    }
}
