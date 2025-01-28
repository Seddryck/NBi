using System.Collections.Generic;
using System.Xml.Serialization;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Constraints;

namespace NBi.Xml.Systems;

public class ExecutionXml : AbstractSystemUnderTestXml, IReferenceFriendly
{       
    public virtual bool IsQuery()
    {
        return true;
    }
    
    [XmlElement(Type = typeof(QueryXml), ElementName = "query"),
    XmlElement(Type = typeof(AssemblyXml), ElementName = "assembly"),
    XmlElement(Type = typeof(ReportXml), ElementName = "report"),
    XmlElement(Type = typeof(SharedDatasetXml), ElementName = "shared-dataset"),
    XmlElement(Type = typeof(EtlXml), ElementName = "etl"),
    ]
    public virtual ExecutableXml Item { get; set; }

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

    public void AssignReferences(IEnumerable<ReferenceXml> references)
    {
        if (Item is IReferenceFriendly)
            ((IReferenceFriendly)Item).AssignReferences(references);
    }
}
