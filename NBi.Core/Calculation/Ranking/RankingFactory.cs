using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Ranking
{
    public class RankingFactory
    {
        public AbstractRanking Instantiate(IRankingInfo info)
        {
            switch (info.Option)
            {
                case RankingOption.Top:
                    return new TopRanking(info.Count, info.Operand, info.Type);
                case RankingOption.Bottom:
                    return new BottomRanking(info.Count, info.Operand, info.Type);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
