using NBi.Core.Members;
using NBi.Core.Members.Predefined;
using ContainedIn = NBi.Model.Constraints.ContainedIn;
using NBi.Model.Items;
using NBi.Model.Items.Ranges;
using NUnit.Framework;

namespace NBi.Core.Testing.Members;

public class MemberSelectionTest
{
    [Test]
    public void GetItems_ModelConstraint_CombinesTheConfiguredSourcesInOrder()
    {
        var constraint = new ContainedIn
        {
            Items = ["Initial item"],
            PredefinedItems = new PredefinedItems { Type = PredefinedMembers.DaysOfWeek, Language = "en-US" },
            IntegerRange = new IntegerRange { Start = 1, End = 2 },
        };

        var items = MemberSelection.GetItems(constraint).ToArray();

        Assert.That(items, Is.EqualTo(new[]
        {
            "Initial item", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", "1", "2",
        }));
    }
}
