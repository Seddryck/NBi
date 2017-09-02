using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Analyzer
{
    public class AnalyzersFactory
    {
        public IEnumerable<IRowsAnalyzer> Instantiate(ComparisonKind kind)
        {
            var list = new List<IRowsAnalyzer>();
            list.Add(new KeyMatchingRowsAnalyzer());

            switch (kind)
            {
                case ComparisonKind.SubsetOf:
                    list.Add(new UnexpectedRowsAnalyzer());
                    break;
                case ComparisonKind.SupersetOf:
                    list.Add(new MissingRowsAnalyzer());
                    break;
                case ComparisonKind.EqualTo:
                    list.Add(new MissingRowsAnalyzer());
                    list.Add(new UnexpectedRowsAnalyzer());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return list;
        }

        public static IEnumerable<IRowsAnalyzer> EqualTo()
        {
            return new AnalyzersFactory().Instantiate(ComparisonKind.EqualTo);
        }
    }
}
