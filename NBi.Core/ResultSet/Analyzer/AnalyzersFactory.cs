using NBi.Core.ResultSet.Equivalence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Analyzer;

public class AnalyzersFactory
{
    public IEnumerable<IRowsAnalyzer> Instantiate(EquivalenceKind kind)
    {
        var list = new List<IRowsAnalyzer>() { new KeyMatchingRowsAnalyzer() };

        switch (kind)
        {
            case EquivalenceKind.IntersectionOf:
                break;
            case EquivalenceKind.SubsetOf:
                list.Add(new UnexpectedRowsAnalyzer());
                break;
            case EquivalenceKind.SupersetOf:
                list.Add(new MissingRowsAnalyzer());
                break;
            case EquivalenceKind.EqualTo:
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
        return new AnalyzersFactory().Instantiate(EquivalenceKind.EqualTo);
    }
}
