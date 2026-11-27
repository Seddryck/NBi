using NBi.Core.Members.Predefined;
using NBi.Core.Members.Ranges;
using NBi.Model.Constraints;

namespace NBi.Core.Members;

public static class MemberSelection
{
    public static IEnumerable<string> GetItems(AbstractConstraintForCollection constraint)
        => GetItems(constraint.Items, constraint.PredefinedItems, constraint.Range);

    public static IEnumerable<string> GetItems(IEnumerable<string>? explicitItems,
        Model.Items.PredefinedItems? predefined, IRange? range)
    {
        var items = explicitItems?.ToList() ?? [];
        if (predefined != null)
            items.AddRange(new PredefinedMembersFactory().Instantiate(predefined.Type, predefined.Language));
        if (range != null)
            items.AddRange(new RangeMembersFactory().Instantiate(range));
        return items;
    }
}