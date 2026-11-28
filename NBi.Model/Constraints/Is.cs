
namespace NBi.Model.Constraints;

public class Is : AbstractConstraint
{
    private string value;
    public string Value
    {
        get { return value.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", ""); }
        set { this.value=value; }
    }
}
