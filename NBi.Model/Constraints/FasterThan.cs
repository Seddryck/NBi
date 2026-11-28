
namespace NBi.Model.Constraints;

public class FasterThan : AbstractConstraint
{
    public int MaxTimeMilliSeconds { get; set; }

    public bool CleanCache { get; set; }

    public int TimeOutMilliSeconds { get; set; }
}
