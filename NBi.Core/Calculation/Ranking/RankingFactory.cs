using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Ranking
{
    public class RankingFactory
    {
        public AbstractRanking Instantiate(RankingArgs args)
        {
            return args.Option switch
            {
                RankingOption.Top => new TopRanking(args.Count, args.Operand, args.Type),
                RankingOption.Bottom => new BottomRanking(args.Count, args.Operand, args.Type),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
