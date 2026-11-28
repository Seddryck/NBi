
namespace NBi.Model.Constraints;

public class Count : AbstractConstraint
{
    protected int _exactly;
    public int Exactly
    {
        get
        { return _exactly; }

        set
        {
            _exactly = value;
            ExactlySpecified = true;
        }
    }

    protected int _moreThan;
    public int MoreThan
    {
        get
        { return _moreThan; }

        set
        {
            _moreThan = value;
            MoreThanSpecified = true;
        }
    }

    protected int _lessThan;
    public int LessThan
    {
        get
        { return _lessThan; }

        set
        {
            _lessThan = value;
            LessThanSpecified = true;
        }
    }

    public bool LessThanSpecified { get; set; }
    public bool MoreThanSpecified { get; set; }
    public bool ExactlySpecified { get; set; }

    public Count()
    {
    }
}
