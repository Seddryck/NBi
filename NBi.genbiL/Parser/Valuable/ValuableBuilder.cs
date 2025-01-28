using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Parser.Valuable;

public class ValuableBuilder
{
    public IEnumerable<IValuable> Build(ValuableType type, IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            yield return Build(type, item);
        }
    }

    public IValuable Build(ValuableType type, string item)
    {
        switch (type)
        {
            case ValuableType.Value: return new Value(item);
            case ValuableType.Column: return new Column(item);
            default:
                throw new ArgumentException();
        }
    }
}
