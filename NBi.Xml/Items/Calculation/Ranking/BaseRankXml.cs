using NBi.Core.Calculation.Ranking;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Items.Calculation.Ranking;

public abstract class BaseRankXml
{
    [XmlAttribute("count")]
    [DefaultValue(1)]
    public int Count { get; set; }

    [XmlIgnore]
    internal abstract RankingOption Option { get; }

    public BaseRankXml()
    {
        Count = 1;
    }
}

public class TopRankingXml : BaseRankXml
{
    internal override RankingOption Option { get => RankingOption.Top; }
}
public class BottomRankingXml : BaseRankXml
{
    internal override RankingOption Option { get => RankingOption.Bottom; }
}
