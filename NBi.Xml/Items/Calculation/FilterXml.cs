using NBi.Core.Evaluate;
using NBi.Xml.Items.Alteration;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Items.Calculation.Ranking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation;

public class FilterXml : AlterationXml
{
    [XmlIgnore()]
    public IReadOnlyCollection<IColumnAlias> Aliases
    {
        get
        {
            return new ReadOnlyCollection<IColumnAlias>(internalAliases.Cast<IColumnAlias>().ToList());
        }
    }

    [XmlElement("alias")]
    public List<AliasXml> InternalAliases
    {
        get { return internalAliases; }
        set { internalAliases = value; }
    }

    [XmlIgnore]
    [Obsolete("Use InternalAlias in place of InternalAliasOld")]
    public List<AliasXml> InternalAliasesOld
    {
        get { return internalAliases; }
        set { internalAliases = value; }
    }

    private List<AliasXml> internalAliases;

    [XmlElement("expression")]
    public ExpressionXml Expression { get; set; }

    [XmlElement("predicate")]
    public SinglePredicationXml Predication { get; set; }

    [XmlElement("ranking")]
    public RankingXml Ranking { get; set; }
    [XmlElement("unique")]
    public UniqueXml Uniqueness { get; set; }

    [XmlElement("combination")]
    public CombinationPredicationXml Combination { get; set; }

    //[XmlElement("ranking")]
    //public RankingXml Ranking { get; set; }

    public FilterXml()
    {
        internalAliases = new List<AliasXml>();
    }
}
