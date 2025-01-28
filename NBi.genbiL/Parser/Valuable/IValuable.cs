using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Parser.Valuable;

public interface IValuable
{
    string Display { get; }
    string GetValue(DataRow data);
}

public enum ValuableType
{
    Value = 0,
    Column = 1
}
