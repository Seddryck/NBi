using NBi.Core.Calculation.Ranking;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Xml.Items.Calculation.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation.Ranking;

public class RankingXml : IRankingInfo
{
    [XmlAttribute("operand")]
    public string OperandSerialized
    {
        get => Operand?.Label;
        set { Operand = new ColumnIdentifierFactory().Instantiate(value); }
    }

    [XmlIgnore()]
    public IColumnIdentifier Operand { get; set; }

    [XmlAttribute("type")]
    [DefaultValue(ColumnType.Numeric)]
    public ColumnType Type { get; set; }

    [XmlElement(Type = typeof(TopRankingXml), ElementName = "top")]
    [XmlElement(Type = typeof(BottomRankingXml), ElementName = "bottom")]
    public BaseRankXml Rank { get; set; }

    [XmlIgnore]
    public RankingOption Option
    {
        get => Rank.Option;
    }

    [XmlIgnore]
    public int Count
    {
        get => Rank.Count;
    }

    [XmlElement(ElementName = "group-by")]
    public GroupByXml GroupBy { get; set; }

    public RankingXml()
    {
        Type = ColumnType.Numeric;
    }
}
