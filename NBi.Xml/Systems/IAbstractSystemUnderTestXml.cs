using System.Xml.Serialization;
using NBi.Xml.Settings;

namespace NBi.Xml.Systems
{
    public interface IAbstractSystemUnderTestXml
    {
        bool IsStructure();
        bool IsQuery();
        bool IsMembers();
    }
}