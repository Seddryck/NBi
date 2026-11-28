using NBi.Core.Calculation.Ranking;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items.Calculation.Ranking;

public abstract class BaseRank
{
    public int Count { get; set; }

    internal abstract RankingOption Option { get; }

    public BaseRank()
    {
        Count = 1;
    }
}

public class TopRanking : BaseRank
{
    internal override RankingOption Option { get => RankingOption.Top; }
}
public class BottomRanking : BaseRank
{
    internal override RankingOption Option { get => RankingOption.Bottom; }
}
