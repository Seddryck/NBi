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
            switch (args.Option)
            {
                case RankingOption.Top:
                    return new TopRanking(args.Count, args.Operand, args.Type);
                case RankingOption.Bottom:
                    return new BottomRanking(args.Count, args.Operand, args.Type);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
