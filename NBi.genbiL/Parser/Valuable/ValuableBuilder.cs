using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Parser.Valuable
{
    public class ValuableBuilder
    {
        public IEnumerable<IValuable> Build(ValuableType type, IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                switch (type)
                {
                    case ValuableType.Value: yield return new Value(item);
                        break;
                    case ValuableType.Column: yield return new Column(item);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }
    }
}
