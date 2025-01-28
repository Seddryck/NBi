using System.Xml.Serialization;

namespace NBi.Xml.Constraints;

public class CountXml : AbstractConstraintXml
{
    
    protected int _exactly; 
    [XmlAttribute("exactly")]
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
    [XmlAttribute("more-than")]
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
    [XmlAttribute("less-than")]
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

    [XmlIgnore]
    public bool LessThanSpecified { get; set; }
    [XmlIgnore]
    public bool MoreThanSpecified { get; set; }
    [XmlIgnore]
    public bool ExactlySpecified { get; set; }

    public CountXml()
    {
    }

}
